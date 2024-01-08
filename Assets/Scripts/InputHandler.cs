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

    private int countVar = 0;
    private int sumFromEntries;
    
    private void Start () {
        inEntries = FileHandler.ReadListFromJSON<InputEntry> (inputFilename);
        outEntries = FileHandler.ReadListFromJSON<OutputEntry>(outputFilename);
        foreach( var x in inEntries) {
            Debug.Log( x);
        }
        
        randomButton.onClick.AddListener(SearchByIdKlang);
        
        // Init
        sumFromEntries = inEntries.Count;
    }

    public void AddNameToList (int claimedDirectionIdBtn)
    {
        string idKlang = idKlang_Output.text;
        
        // Aktuelle Uhrzeit im Format "HH:mm:ss" holen
        string currentTime = DateTime.Now.ToString("HH:mm:ss");
        
        OutputEntry existingEntry = outEntries.Find(entry => entry.id_klang == idKlang);

        switch (claimedDirectionIdBtn)
        {
            case 0:
                claimed_direction = "front";
                claimedDirection_Output.text = claimed_direction;
                break;
            case 1:
                claimed_direction = "front_right";
                claimedDirection_Output.text = claimed_direction;
            break;
            case 2:
                claimed_direction = "right";
                claimedDirection_Output.text = claimed_direction;
            break;
            case 3:
                claimed_direction = "back_right";
                claimedDirection_Output.text = claimed_direction;
            break;
            case 4:
                claimed_direction = "back";
                claimedDirection_Output.text = claimed_direction;
            break;
            case 5:
                claimed_direction = "back_left";
                claimedDirection_Output.text = claimed_direction;
            break;
            case 6:
                claimed_direction = "left";
                claimedDirection_Output.text = claimed_direction;
            break;
            case 7:
                claimed_direction = "front_left";
                claimedDirection_Output.text = claimed_direction;
            break;
            case 8:
                claimed_direction = "front_bottom";
                claimedDirection_Output.text = claimed_direction;
            break;
            case 9:
                claimed_direction = "bottom";
                claimedDirection_Output.text = claimed_direction;
            break;
            case 10:
                claimed_direction = "back_bottom";
                claimedDirection_Output.text = claimed_direction; 
            break;
            case 11:
                claimed_direction = "back_top";
                claimedDirection_Output.text = claimed_direction;
            break;
            case 12:
                claimed_direction = "top";
                claimedDirection_Output.text = claimed_direction;
            break;
            case 13:
                claimed_direction = "front_top";
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
        //string searchIdKlang = Random.Range(0, 2).ToString();
        int randomInt = Random.Range(0, inEntries.Count);
        Debug.Log(randomInt);
        InputEntry result = inEntries[randomInt];
        inEntries.RemoveAt(randomInt);
        //InputEntry result = inEntries.Find(entry => entry.id_klang == searchIdKlang);
        
        
        idKlang_Output.text = "";
        idTestperson_Output.text = "";
        claimedDirection_Output.text = "";
        
        // Debug.Log (GetPath (inputFilename));
    
        
        if (result != null)
        {
            idKlangText_Input.text = $"ID_Klang: {result.id_klang}";
            idKlang_Output.text = $"{result.id_klang}";
            //source.PlayOneShot(audioClips[int.Parse(searchIdKlang)]);
            string path = GetPath(".");
            //string filePath = path.Substring(0, path.Length - 2) + "/" + result.name_wav_file;
            string filePath = result.name_wav_file;
            //Debug.Log(filePath);
            AudioClip audioClip = Resources.Load<AudioClip>(filePath.Replace(".wav", ""));

            countVar++;
            Debug.Log("Sound-Nummer: " + countVar + "/" + sumFromEntries);
            
            if (audioClip == null)
            {
                Debug.LogError("Failed to load WAV file: " + result.name_wav_file);
                return;
            }

            //source.clip = audioClip;

            // source.Play();
            
            //source.PlayOneShot(klang_clip);
            //source.PlayOneShot(audioClips[randomInt]);
            
            source.PlayOneShot(audioClip);

            
            idTestpersonText_Input.text = $"ID_Testperson: {result.id_testperson}";
            idTestperson_Output.text = $"{result.id_testperson}";
            // Debug.Log(result.name_wav_file);   
            nameWavFileText_Input.text = $"Name Wav File: {result.name_wav_file}";
                
            directionText_Input.text = $"Direction: {result.direction}";
            actual_direction = $"{result.direction}";
           
        }
        else
        {
            idKlangText_Input.text = $"Entry not found.";
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