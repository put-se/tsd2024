using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace GoldSavings.App.Model;

public class GoldPrice
{
    [JsonProperty("Data")]
    public DateTime Date { get; set; }

    [JsonProperty("Cena")]
    public double Price { get; set; }
}