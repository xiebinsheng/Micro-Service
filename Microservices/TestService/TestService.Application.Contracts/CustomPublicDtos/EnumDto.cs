using System;
using System.Collections.Generic;
using System.Text;

namespace TestService.Application.Contracts.CustomPublicDtos
{
    public class EnumDto
    {
        public EnumDto(string field, int value, string description)
        {
            Key = field;
            Value = value;
            Label = description;
        }

        public string Key { get; set; }

        public int Value { get; set; }

        
        public string Label { get; set; }
    }
}
