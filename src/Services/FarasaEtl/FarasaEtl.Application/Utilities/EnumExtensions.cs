﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarasaEtl.Application.Utilities;
public static class EnumExtensions
{
    public static string GetEnumDescription(this Enum enumValue)
    {
        try
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                return attribute.Description;
            }
            throw new ArgumentException("Item not found.", nameof(enumValue));
        }
        catch
        {

            throw;
        }
        
        //throw new ArgumentException("Item not found.", nameof(enumValue));
    }
}
