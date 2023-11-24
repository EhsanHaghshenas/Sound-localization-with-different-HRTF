using System;

[Serializable]
public class OutputEntry {
    public string id_klang;
    public string id_testperson;
    public string claimed_direction;
    public string actual_direction;
    public string timestamp;

    public OutputEntry (string iK, string idTP, string cD, string aD, string tS) {
        id_klang = iK;
        id_testperson = idTP;
        claimed_direction = cD;
        actual_direction = aD;
        timestamp = tS;
    }
}

[Serializable]
public class InputEntry {
    public string id_klang;
    public string id_testperson;
    public string name_wav_file; // Hinzugefügt, um die Struktur anzugleichen
    public string direction; // Hinzugefügt, um die Struktur anzugleichen

    public InputEntry(string idKL, string idTPN, string nWF, string d)
    {
        id_klang = idKL;
        id_testperson = idTPN;
        name_wav_file = nWF;
        direction = d;
    }
}