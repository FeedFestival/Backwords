using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[ExecuteInEditMode]
public class DomainLogic : MonoBehaviour
{
    public void RecreateDataBase()
    {
        var dataService = new DataService("Database.db");
        dataService.CreateDB();
    }

    public void CleanUpUsers()
    {
        var dataService = new DataService("Database.db");
        dataService.CleanUpUsers();
    }

    public void LoadLevelsFromCsv()
    {
        var csvReader = GetComponent<CSVReader>();

        var csvGrid = csvReader.SplitCsvGrid(null);

        var levels = new List<Level>();

        for (int row = 1; row < csvGrid.GetLength(1); row++)
        {
            if (string.IsNullOrEmpty(csvGrid[2, row]))
                break;

            int id;
            Int32.TryParse(csvGrid[0, row], out id);

            var level = new Level
            {
                Id = id,
                Image = csvGrid[1, row],
                Romana = csvGrid[2, row],
                Engleza = csvGrid[3, row],
                Portugheza = csvGrid[4, row]
            };

            levels.Add(level);
        }

        var dataService = new DataService("Database.db");
        dataService.CleanUpLevels();

        foreach (Level l in levels)
        {
            dataService.CreateLevel(l);
        }
    }
}