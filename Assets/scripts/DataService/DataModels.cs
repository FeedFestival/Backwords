using System.Collections.Generic;
using SQLite4Unity3d;
using UnityEngine;
using System.Collections;
using System;
using System.Net;
using Assets.scripts.utils;

public class User
{

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    public string Name { get; set; }
    public int Maps { get; set; }

    // Game Settings
    public bool IsUsingSound { get; set; }

    //[Ignore]
    //public FacebookApp FacebookApp { get; set; }
    
    public override string ToString()
    {
        return string.Format(@"
            [User: 
            Id={0}, 
            Name={1}, 
            Maps={2}, 
            IsUsingSound={3}", 
            Id, 
            Name, 
            Maps, 
            IsUsingSound);
    }
    
    public static User FillData(string properties)
    {
        return new User
        {
            Id = utils.GetIntDataValue(properties, "ID:"),
            Name = utils.GetDataValue(properties, "Name:"),
            Maps = utils.GetIntDataValue(properties, "Maps:"),
            IsUsingSound = utils.GetBoolDataValue(properties, "IsUsingSound:")
            //FacebookApp = new FacebookApp
            //{
            //    FacebookId = utils.GetLongDataValue(properties, "FacebookId:")
            //}
        };
    }
}

public class Level
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Word { get; set; }
}