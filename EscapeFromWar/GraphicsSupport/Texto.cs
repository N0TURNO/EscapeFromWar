using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EscapeFromWar.GraphicsSupport
{
   static public class TextoCust
    {
        static private SpriteFont letra = null;//Tipo de letra a ser usada.
        static private Color defaultCorTexto = Color.Crimson;//Cor do texto por defeito.
        static private Vector2 localizacaoTexto = new Vector2(5, 5);//Local onde o texto vai ser print por definicao.


        static private void CarregaLetra()
        {
            if (null == letra)
                letra = Game1.content.Load<SpriteFont>("Arial");
            //Loads o tipo de letra do tipo  arial.
        }

        static private Color CorAusar(Nullable<Color> c)
        {//Verifica se existe uma cor seleccionada caso isso nao se verifique usa a cor predefinida.
            return (null == c) ? defaultCorTexto : (Color)c;
        }

        static public void PrintTextoLocalizacao(Vector2 posicao, String mensagem, Nullable<Color> cor )
        {//Local onde vai ser carregado texto numa localizacao dada.
            CarregaLetra();

            Color corUsar = CorAusar(cor);

            int pixelX, pixelY;
            Camera.ConvercaoParaPixeisPosicao(posicao, out pixelX, out pixelY);
            Game1.spriteBatch.DrawString(letra, mensagem, new Vector2(pixelX, pixelY), corUsar);
        }

        static public void PrintTexto(String mensagem, Nullable<Color> cor)
        {//Localizacao predifinida para o texto.
            CarregaLetra();

            Color corUsar = CorAusar(cor);

            Game1.spriteBatch.DrawString(letra, mensagem, localizacaoTexto, corUsar);
        }

    }
}
