using System.Collections.Generic;
using UnityEngine;

public static class TypeOfItemExtensions
{
    private static readonly Dictionary<TipoDeItem, string> nomesPortugues = new Dictionary<TipoDeItem, string>
    {
        { TipoDeItem.ChaveAmarela, "Chave Amarela" },
        { TipoDeItem.ChaveVerde, "Chave Verde" },
        { TipoDeItem.ChaveVermelha, "Chave Vermelha" },
        { TipoDeItem.ChaveCircular, "Chave Circular" },
        { TipoDeItem.PeDeCabra, "Pé de Cabra" },
        { TipoDeItem.Alicate, "Alicate" },
        { TipoDeItem.GrampoDeCabelo, "Grampo de Cabelo" },
        { TipoDeItem.Carne, "Carne" },
        { TipoDeItem.Desinfetante, "Amaciante" },
        { TipoDeItem.Porta, "Porta" },
        { TipoDeItem.Crucifixo, "Crucifixo" },
        { TipoDeItem.ChaveQuadrada, "Chave Quadrada" },
        { TipoDeItem.Gaveta, "Gaveta" },
        { TipoDeItem.Senha, "Senha" }
        
    };

    private static readonly Dictionary<TipoDeItem, string> nomesIngles = new Dictionary<TipoDeItem, string>
    {
        { TipoDeItem.ChaveAmarela, "Yellow Key" },
        { TipoDeItem.ChaveVerde, "Green Key" },
        { TipoDeItem.ChaveVermelha, "Red Key" },
        { TipoDeItem.ChaveCircular, "Round Key" },
        { TipoDeItem.PeDeCabra, "Crowbar" },
        { TipoDeItem.Alicate, "Pliers" },
        { TipoDeItem.GrampoDeCabelo, "Hairpin" },
        { TipoDeItem.Carne, "Meat" },
        { TipoDeItem.Desinfetante, "softener" },
        { TipoDeItem.Porta, "Door" },
        { TipoDeItem.Crucifixo, "Crucifix" },
        { TipoDeItem.ChaveQuadrada, "Square Hey" },
        { TipoDeItem.Gaveta, "Drawer" },
        { TipoDeItem.Senha, "Password" }
    };

    public static string ParaNomeLegivel(this TipoDeItem tipo)
    {
        int idioma = PlayerPrefs.GetInt("Language"); // 0 = pt, 1 = en

        var nomes = idioma == 1 ? nomesIngles : nomesPortugues;

        return nomes.TryGetValue(tipo, out var nome) ? nome : tipo.ToString();
    }
}
