﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TimeWallet_Mobile_.Data.Models;

namespace TimeWallet_Mobile_.Data.DTO_s.Json
{
    public class UserDTO
    {
        [JsonPropertyName("user")]
        public Models.User user { get; set; }
    }
}
