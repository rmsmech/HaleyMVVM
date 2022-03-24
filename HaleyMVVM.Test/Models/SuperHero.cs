﻿using Haley.MVVM;
using System;
using System.Collections.Generic;
using System.Text;
using Haley.IOC;
using Haley.Models;

namespace HaleyMVVM.Test.Models
{
    /// <summary>
    /// SuperHero is an extension of a Person who has a secret identity
    /// </summary>
    public class SuperHero : Person
    {
        public int value { get; private set; }
        [HaleyInject]
        public string power { get; set; }
        public string alter_ego { get; set; }
        public void IncreasePower(int count) 
        {
            for (int i = 0; i < count; i++)
            {
                value++;
            }
        }
    }
}
