using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public static class NameGenerator
{
    public static readonly string[] CommonFirstNames = new string[]
    {
        "Andrei","Alexandru","Mihai","Ion","Vasile","Daniel","Gabriel","Adrian","Cristian","Florin",
        "George","Ionu?","Marian","Nicolae","C?t?lin","Claudiu","Dumitru","Emil","Eugen","Lauren?iu",
        "Lucian","Marius","Ovidiu","Paul","Radu","Robert","Sorin","?tefan","Tudor","Valentin",
        "Victor","Alin","Bogdan","Ciprian","Constantin","Costin","Drago?","Gheorghe","Ilie","Iulian",
        "Liviu","Octavian","Petru","Rare?","Silviu","Teodor","Viorel","Aurel","Cornel","Darius",
        "Emanuel","Hora?iu","Leontin","Marcel","Nicu","Sebastian","Traian","Virgil"
    };

    public static readonly string[] CommonSurnames = new string[]
    {
        "Popescu","Ionescu","Popa","Stan","Dumitrescu","Georgescu","Marin","Voicu","Dobre","Moldovan",
        "Ilie","Radu","Munteanu","Stoica","Preda","Lazar","Matei","Constantinescu","Diaconu","Petrescu",
        "Serban","Barbu","Neagu","Florea","Albu","Enache","Dragomir","Coman","Manole","Toma",
        "Badea","Costache","Cristea","Grigore","Sandu","Pavel","Voinea","Oprea","Anghel","Mihalache",
        "Stancu","Nistor","Rosu","Dascalu","Pop","Moraru","Morar","Ardelean","Chirila","Fieraru",
        "Cojocaru","Avram","Balan","Filip","Hagi","Ignat","Jianu","Lungu","Mazilu","Onofrei"
    };

    public static HashSet<string> namesGenerated = new HashSet<string>();

    public static string GetRandomName()
    {
        string newName;

        do
        {
            string first = CommonFirstNames[UnityEngine.Random.Range(0, CommonFirstNames.Length)];
            string last = CommonSurnames[UnityEngine.Random.Range(0, CommonSurnames.Length)];

            newName = first + " " + last;

        } while (namesGenerated.Contains(newName));

        namesGenerated.Add(newName);
        return newName;
    }
}