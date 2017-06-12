using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EscapeFromWar.GraphicsSupport
{
   public class MostraVector
    {
        protected static Texture2D imagemVector = null;//Imagem do vector.
        private static float ratioImagemVector = 0.2f;//Ratio da imagem.

        static private void CarregaImagem()
        {
            if (null == imagemVector)
                MostraVector.imagemVector = Game1.content.Load<Texture2D>("Arrow");
            //carrega imagem do vector.
        }

        static public void PontoOrigemVector(Vector2 origem, Vector2 direcao)
        {
            CarregaImagem();

            #region Angulo de rotacao
            float comprimento = direcao.Length();
            float anguloTheta = 0f;

            if(comprimento>0.001f)
            {
                direcao /= comprimento;
                anguloTheta = (float)Math.Acos((double)direcao.X);

                if(direcao.X<float.Epsilon)
                {
                    if (direcao.Y > float.Epsilon)
                        anguloTheta = -anguloTheta;
                }
                else
                {
                    if (direcao.Y > float.Epsilon)
                        anguloTheta = -anguloTheta;
                }
            }
            #endregion
            # region Print seta 
            //tamanho do objecto e local.
            Vector2 tamanho = new Vector2(comprimento, ratioImagemVector * comprimento);
            Rectangle areaJogo = Camera.ConvercaoParaPixeisRectangulo(origem, tamanho);

            Vector2 og = new Vector2(0f, MostraVector.imagemVector.Height / 2f);

            Game1.spriteBatch.Draw(MostraVector.imagemVector, areaJogo, null, Color.AntiqueWhite, anguloTheta, og, SpriteEffects.None, 0f);

            #endregion

            #region print message
            String msg;
            msg = "Direção=" + direcao + "\nTamanho" + comprimento;
            TextoCust.PrintTextoLocalizacao(origem + new Vector2(2, 5), msg, Color.Bisque);
            #endregion
        }

        static public void PrintOrigPara(Vector2 localDe, Vector2 localPara)
        {
            PontoOrigemVector(localDe, localPara - localDe);
        }

        static public Vector2 ModificarPeloAng(Vector2 v, float angRadianos )
        {
            float senTheta=(float)(Math.Sin((double)angRadianos));
            float cosTheta = (float)(Math.Cos((double)angRadianos));
            float x, y;
            x = cosTheta * v.X + senTheta * v.Y;
            y = -senTheta * v.X + cosTheta * v.Y;
            return new Vector2(x, y);
        }
    }
}
