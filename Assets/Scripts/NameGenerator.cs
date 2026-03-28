using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public static class NameGenerator
{
    public static readonly string[] CommonFirstNames = new string[]
    {
        "James","John","Robert","Michael","William","David","Richard","Joseph","Thomas","Charles",
        "Daniel","Matthew","Anthony","Mark","Steven","Paul","Andrew","Joshua","Kenneth","Kevin",
        "Brian","George","Edward","Ronald","Timothy","Jason","Jeffrey","Ryan","Jacob","Gary",
        "Nicholas","Eric","Jonathan","Stephen","Larry","Justin","Scott","Brandon","Benjamin","Samuel",
        "Gregory","Alexander","Patrick","Frank","Raymond","Jack","Dennis","Jerry","Tyler","Aaron",
        "Henry","Douglas","Peter","Nathan","Zachary","Kyle","Adam","Carl","Arthur"
    };
    public static readonly string[] CommonSurnames = new string[]
    {
        "Smith",
        "Johnson",
        "Williams",
        "Brown",
        "Jones",
        "Garcia",
        "Miller",
        "Davis",
        "Rodriguez",
        "Martinez",

        "Hernandez",
        "Lopez",
        "Gonzalez",
        "Wilson",
        "Anderson",
        "Thomas",
        "Taylor",
        "Moore",
        "Jackson",
        "Martin",

        "Lee",
        "Perez",
        "Thompson",
        "White",
        "Harris",
        "Sanchez",
        "Clark",
        "Ramirez",
        "Lewis",
        "Robinson",

        "Walker",
        "Young",
        "Allen",
        "King",
        "Wright",
        "Scott",
        "Torres",
        "Nguyen",
        "Hill",
        "Flores",

        "Green",
        "Adams",
        "Nelson",
        "Baker",
        "Hall",
        "Rivera",
        "Campbell",
        "Mitchell",
        "Carter",
        "Roberts",

        "Gomez",
        "Phillips",
        "Evans",
        "Turner",
        "Diaz",
        "Parker",
        "Cruz",
        "Edwards",
        "Collins",
        "Reyes"
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