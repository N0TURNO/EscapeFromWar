using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EscapeFromWar.GraphicsSupport
{
    public partial class Texturas
    {
        private Color[] corTexturas = null;// Array para guardar as cores de cada pixel das texturas.

        #region suporte estatico para imagens com cores iguais no seu interior.

        static Dictionary<String, Color[]> dadosTextura = new Dictionary<string, Color[]>();
        // Para guardar no mesmo index colores iguais dentro da imagem.
        static private Color[] CarregaCor(String nomeI, Texture2D imagem)
        {//verifica as cores de uma imagem coloca dentro de uma array e esse array e nome da imagem sao adicionados a um dicionario.
            Color[] dadosImagem = new Color[imagem.Width * imagem.Height];
            imagem.GetData(dadosImagem);
            dadosTextura.Add(nomeI, dadosImagem);
            return dadosImagem;
        }
    

        #endregion

        private void LeDadosCor()
        {// Vai buscar a cor à imagem verifica se exite no dicionario e se nao adiciona a esse mesmo dicionario.
            if (dadosTextura.ContainsKey(nomeAVisual))
                corTexturas = dadosTextura[nomeAVisual];
            else
                corTexturas = CarregaCor(nomeAVisual, assetVisual);
        }

        private Color BuscaCores(int i, int j)
        {//Ir buscar cor de um pixel expecifico de uma Sprite.
            return corTexturas[((j + SpritePixeltopoSup) * assetVisual.Width) + i + SpritePixelbaixoEsq];
        }

        public bool PixeisTocaram(Texturas outraTextura, out Vector2 pontoColisao)
        {
            bool toca = ToqueTextures(outraTextura);
            pontoColisao = Posicao;
            if (toca)
            {
                bool toquePixel = false;

                Vector2 direccaoIX = MostraVector.ModificarPeloAng(Vector2.UnitX, Rotacao);
                Vector2 direccaoIY = MostraVector.ModificarPeloAng(Vector2.UnitY, Rotacao);

                Vector2 outraDirX = MostraVector.ModificarPeloAng(Vector2.UnitX, outraTextura.Rotacao);
                Vector2 outraDirY = MostraVector.ModificarPeloAng(Vector2.UnitY, outraTextura.Rotacao);

                int i = 0;
                while ((!toquePixel) && (i < SpriteLargura))

                {
                    int j = 0;
                    while ((!toquePixel) && (j < SpriteAltura))
                    {
                        pontoColisao = IndiceDaPosicaoCamera(i, j, direccaoIX, direccaoIY);
                        Color corI = BuscaCores(i, j);
                        if (corI.A > 0)
                        {
                            Vector2 outroIndex = outraTextura.PosicaoCameraIndex(pontoColisao, outraDirX, outraDirY);
                            int xMin = (int)outroIndex.X;
                            int yMin = (int)outroIndex.Y;

                            if ((xMin >= 0) && (xMin < outraTextura.SpriteLargura) && (yMin >= 0) && (yMin < outraTextura.SpriteAltura))
                            {
                                toquePixel = (outraTextura.BuscaCores(xMin, yMin).A > 0);
                            }
                        }
                        j++;
                    }
                    i++;

                }
                toca = toquePixel;
            }
            return toca;
        }

        private Vector2 IndiceDaPosicaoCamera(int i, int j, Vector2 direcaoX, Vector2 direcaoY)
        {
            float x = i * Largura / (float)(SpriteLargura - 1);
            float y = j * Altura / (float)(SpriteAltura - 1);

            Vector2 novoVector = Posicao + (x - (tamanhoI.X * 0.5f)) * direcaoX - (y - (tamanhoI.Y * 0.5f)) * direcaoY;

            return novoVector; 
        }

        private Vector2 PosicaoCameraIndex(Vector2 p, Vector2 DireccaoX, Vector2 DireccaoY)
        {
            Vector2 delta = p - Posicao;
            float xOffset = Vector2.Dot(delta, DireccaoX);
            float yOffset = Vector2.Dot(delta, DireccaoY);
            float i = SpriteLargura * (xOffset / Largura);
            float j = SpriteAltura * (yOffset / Altura);
            i += SpriteLargura / 2;
            j = (SpriteAltura / 2) - j;
            return new Vector2(i, j);
        }

    }
}
