﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using Haley.Abstractions;
using Haley.Enums;
using Haley.WPF.Controls;
using Haley.Utils;
using System.Windows;
using System.Windows.Controls;
using Haley.MVVM;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Haley.Models;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Markup;

namespace Haley.Utils
{
    public class LangExtension : MarkupExtension
    {
        private string _resourceKey;
        //[ConstructorArgument("key")] //This attribute has no value. It is not affected by any means.
        public string ResourceKey
        {
            get { return _resourceKey; }
            set { _resourceKey = value; }
        }

        public string ProviderKey { get; set; } //not mandatory. This is to be used when the provider is from a different assembly.

        public LangExtension(string key)
        {
            _resourceKey = key;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            //THIS WHOLE PART IS JUST A MIDDLE WARE. WHEN THE XAML REQUESTS FOR A VALUE USING "PROVIDE VALUE" METHOD OF THE EXTENSION, WE MERELY CREATE A NEW PROPERTY BINDING AND USE THE NEW BINDING'S PROVIDE VALUE.

            #region Option 1
            //We can even create an attached property on the target element and fetch that data here (one alternative)
            ////If we wish to get datacontext of the binding element (like textbloc or box, use below methods to get the prop and then the property where it is bounded)
            //var targetProvider = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            //var targetElement = targetProvider.TargetObject as FrameworkElement; //This element will have a Tag value (which can be used to store the required data)
            //var targetProperty = targetProvider.TargetProperty as DependencyProperty;
            #endregion

            string provider_key = ProviderKey;

            if (string.IsNullOrWhiteSpace(provider_key))
            {
                //It should always be left null. But in case, it is not left null, then use this.
                Uri callerUri = ((IUriContext)serviceProvider.GetService(typeof(IUriContext)))?.BaseUri;
                if (callerUri != null)
                {
                    //var _splitted = callerUri.AbsolutePath.Split(new string[] { ";component" }, StringSplitOptions.None); //It will be in FullPack URI format.
                    //if (_splitted.Length > 1)
                    //{
                    //    //We need more than one
                    //    var _asmName = _splitted[0].Substring(1, _splitted[0].Length - 1); //Remove the backslash.
                    //    if (!string.IsNullOrWhiteSpace(_asmName))
                    //    {
                    //        provider_key = _asmName;
                    //    }
                    //}

                    var _splitted = callerUri.Segments[1].Split(new string[] { ";component" }, StringSplitOptions.None); 
                    if (_splitted.Length > 1)
                    {
                        //We need more than one
                        var _asmName = _splitted[0]; //Remove the backslash.
                        if (!string.IsNullOrWhiteSpace(_asmName))
                        {
                            provider_key = _asmName;
                        }
                    }
                }
            }
            
           //NOW CREATE A NEW BINDING AND SET A NEW PROVIDE VALUE.
            var binding = new Binding("Value") //ResourceData contains the property "Value" which will return the translated value for the given key.
            {
                Source = new ResourceData(_resourceKey, provider_key)
            };
            return binding.ProvideValue(serviceProvider);
        }
    }
}