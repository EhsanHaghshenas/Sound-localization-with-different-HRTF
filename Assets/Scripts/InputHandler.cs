using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InputHandler : MonoBehaviour {
    [SerializeField] Text idKlangText_Input;
    [SerializeField] Text idTestpersonText_Input;
    [SerializeField] Text nameWavFileText_Input;
    [SerializeField] Text directionText_Input;
    
    [SerializeField] Text idKlang_Output;
    [SerializeField] Text idTestperson_Output;
    [SerializeField] Text claimedDirection_Output;
    
    [SerializeField] Button randomButton;
    
    [SerializeField] string inputFilename;
    [SerializeField] string outputFilename;

    public AudioClip[] audioClips;
    public AudioSource source;
    
    private string claimed_direction = "";
    private string actual_direction = "";
    
    List<InputEntry> inEntries = new List<InputEntry> ();
    List<OutputEntry> outEntries = new List<OutputEntry>();

    private void Start () {
        inEntries = FileHandler.ReadListFromJSON<InputEntry> (inputFilename);
        outEntries = FileHandler.ReadListFromJSON<OutputEntry>(outputFilename);
        
        randomButton.onClick.AddListener(SearchByIdKlang);
    }

    public void AddNameToList (int claimedDirectionIdBtn)
    {
        string idKlang = idKlang_Output.text;
        
        // Aktuelle Uhrzeit im Format "HH:mm:ss" holen
        string currentTime = DateTime.Now.ToString("HH:mm:ss");
        
        OutputEntry existingEntry = outEntries.Find(entry => entry.id_klang == idKlang);

        switch (claimedDirectionIdBtn)
        {
            case 1:
                claimed_direction = "Horizontal Front";
                claimedDirection_Output.text = claimed_direction;
            break;
            case 2:
                claimed_direction = "Horizontal Front Left";
                claimedDirection_Output.text = claimed_direction;
            break;
            case 3:
                claimed_direction = "Horizontal Left";
                claimedDirection_Output.text = claimed_direction;
            break;
            case 4:
                claimed_direction = "Horizontal Back Left";
                claimedDirection_Output.text = claimed_direction;
            break;
            case 5:
                claimed_direction = "Horizontal Back";
                claimedDirection_Output.text = claimed_direction;
            break;
            case 6:
                claimed_direction = "Horizontal Back Right";
                claimedDirection_Output.text = claimed_direction;
            break;
            case 7:
                claimed_direction = "Horizontal Right";
                claimedDirection_Output.text = claimed_direction;
            break;
            case 8:
                claimed_direction = "Horizontal Front Right";
                claimedDirection_Output.text = claimed_direction;
            break;
            case 9:
                claimed_direction = "Top";
                claimedDirection_Output.text = claimed_direction;
            break;
        }

        if (existingEntry == null)
        {
            outEntries.Add(new OutputEntry(idKlang, idTestperson_Output.text, claimed_direction, actual_direction, currentTime));
        
            FileHandler.SaveToJSON<OutputEntry>(outEntries, outputFilename, true);
        }
        else
        {
            Debug.LogWarning("Entry with the same ID sound already exists.");
        }
    }
    
    public void SearchByIdKlang()
    {
        // Generiere eine zufällige Zahl zwischen 0 und 26 und setze sie als searchIdKlang
        string searchIdKlang = Random.Range(0, 7).ToString();
        InputEntry result = inEntries.Find(entry => entry.id_klang == searchIdKlang);
        
        idKlang_Output.text = "";
        idTestperson_Output.text = "";
        claimedDirection_Output.text = "";
        
        Debug.Log (GetPath (inputFilename));
        
        if (result != null)
        {
            idKlangText_Input.text = $"ID_Klang: {result.id_klang}";
            idKlang_Output.text = $"{result.id_klang}";
            source.PlayOneShot(audioClips[int.Parse(searchIdKlang)]);
            
            idTestpersonText_Input.text = $"ID_Testperson: {result.id_testperson}";
            idTestperson_Output.text = $"{result.id_testperson}";
                
            nameWavFileText_Input.text = $"Name Wav File: {result.name_wav_file}";
                
            directionText_Input.text = $"Direction: {result.direction}";
            actual_direction = $"{result.direction}";
        }
        else
        {
            idKlangText_Input.text = $"Entry with ID Klang {searchIdKlang} not found.";
            idTestpersonText_Input.text = "";
            nameWavFileText_Input.text = "";
            directionText_Input.text = "";
            
        }
    }
    
    private static string GetPath (string filename) {
        // Verwende Application.dataPath, um den Assets-Ordner zu erhalten
        return Path.Combine(Application.dataPath, filename);
        
        //Der Application.persistentDataPath wird normalerweise für persistente Daten auf dem Gerät verwendet
        //return Application.persistentDataPath + "/" + filename;
    }
}