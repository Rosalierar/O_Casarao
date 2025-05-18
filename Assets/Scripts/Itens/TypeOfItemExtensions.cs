using System.Collections.Generic;
using UnityEngine;

public static class TypeOfItemExtensions
{
    private static readonly Dictionary<TipoDeItem, string> nomesPortugues = new Dictionary<TipoDeItem, string>
    {
        { TipoDeItem.ChaveCircular, "Chave Circular" },
        { TipoDeItem.PeDeCabra, "Pé de Cabra" },
        { TipoDeItem.Alicate, "Alicate" },
        { TipoDeItem.GrampoDeCabelo, "Grampo de Cabelo" },
        { TipoDeItem.Carne, "Carne" },
        { TipoDeItem.Porta, "Porta" },
        { TipoDeItem.PortaCapela, "Porta da Capela" },
        { TipoDeItem.GavetaChave, "Gaveta com Chave" },
        { TipoDeItem.Gaveta, "Gaveta" }
    };

    private static readonly Dictionary<TipoDeItem, string> nomesIngles = new Dictionary<TipoDeItem, string>
    {
        { TipoDeItem.ChaveCircular, "Round Key" },
        { TipoDeItem.PeDeCabra, "Crowbar" },
        { TipoDeItem.Alicate, "Pliers" },
        { TipoDeItem.GrampoDeCabelo, "Hairpin" },
        { TipoDeItem.Carne, "Meat" },
        { TipoDeItem.Porta, "Door" },
        { TipoDeItem.PortaCapela, "Chapel Door" },
        { TipoDeItem.GavetaChave, "Locked Drawer" },
        { TipoDeItem.Gaveta, "Drawer" }
    };

    public static string ParaNomeLegivel(this TipoDeItem tipo)
    {
        int idioma = PlayerPrefs.GetInt("Language"); // 0 = pt, 1 = en

        var nomes = idioma == 1 ? nomesIngles : nomesPortugues;

        return nomes.TryGetValue(tipo, out var nome) ? nome : tipo.ToString();
    }
}
