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
        //Variavies que suportam assets visuais.
        protected Texture2D assetVisual; //Imagem a ser processada;
        protected String nomeAVisual; //Nome do directorio da imagem;
        protected Vector2 posicaoI; //Posicao central da imagem;
        protected Vector2 tamanhoI; //Tmanho da imagem;
        protected float rotacaoI; //Anglo em radianos para ser aplicado na imagem com a direcçao do sentido dos relogios. 
        protected String textoProcessado; //Texto para ser processado.
        protected Color corTexto=Color.Black; // Cor do texto.
        protected Color tintImage;//cor que ira tingir a imagem

        protected void InicializadorVisualAssets(String nome, Vector2 posicao, Vector2 tamanho, String texto = null)
        {
            assetVisual = Game1.content.Load < Texture2D>(nome);
            nomeAVisual = nome;
            tintImage = Color.White;
            posicaoI = posicao;
            tamanhoI = tamanho;
            rotacaoI = 0;
            textoProcessado = texto;
            LeDadosCor(); // Vai buscar as cores presentes numa textura.
        }

        public Texturas (String nome, Vector2 posicao, Vector2 tamanho, String texto = null)
        {
            InicializadorVisualAssets(nome, posicao, tamanho, texto);
        }
        public Texturas(String nome)
        {
            InicializadorVisualAssets(nome,Vector2.Zero, new Vector2(1f, 1f));
        }

        // Accessors
        public Vector2 Posicao { get { return posicaoI; } set { posicaoI = value; } }
        public float PosicaoX { get { return posicaoI.X; } set { posicaoI.X = value; } }
        public float PosicaoY { get { return posicaoI.Y; } set { posicaoI.Y = value; } }
        public Vector2 Tamanho { get { return tamanhoI; } set { tamanhoI = value; } }

        public float Area { get { return this.Largura * this.Altura; } }

        public float Largura { get { return tamanhoI.X; }set { tamanhoI.X = value; } }
        public float Altura { get { return tamanhoI.Y; } set { tamanhoI.Y = value; } }
        //obtemos os limites somando(SUP) ou subtraindo(INF) metade do tamanho da imagem(centro) à posiçao central.
        public Vector2 LimiteMin { get { return posicaoI - (0.5f * tamanhoI); } }
        public Vector2 LimiteMaxx { get { return posicaoI + (0.5f * tamanhoI); } }
        public float Rotacao { get { return rotacaoI; }set { rotacaoI = value; } }
        public String Texto { get { return textoProcessado; }set { textoProcessado = value; } }
        public Color CorTexto { get { return corTexto; }set { corTexto = value; } }

        public void SetTextureImage(Texture2D i) { assetVisual = i; }

        //Acessores para colisao pixel a pixel
        //Limites da Sprite
        protected virtual int SpritePixeltopoSup { get { return 0; } }
        protected virtual int SpritePixelbaixoEsq { get { return 0; } }
        protected virtual int SpriteLargura { get { return assetVisual.Width; } }
        protected virtual int SpriteAltura { get { return assetVisual.Height; } }

        public void Update(Vector2 deltaTranslate, Vector2 deltaScale, float anguloRotacao)
        {//Movimento das texturas
            posicaoI += deltaTranslate;
            tamanhoI += deltaScale;
            rotacaoI += anguloRotacao;
        }
        public bool ToqueTextures(Texturas outraTextura)
        {
            if((Math.Abs(rotacaoI)<float.Epsilon)&&(Math.Abs(outraTextura.rotacaoI)<float.Epsilon))
            {
                //Sem rotaçoes e verifica se exite uma sobreposiçao de limites
                Vector2 meuMin = LimiteMin;
                Vector2 outroMin = outraTextura.LimiteMin;

                Vector2 meuMax = LimiteMaxx;
                Vector2 outroMax = outraTextura.LimiteMaxx;

                return ((meuMin.X < outroMax.X) && (meuMax.X > outroMin.X) && (meuMin.Y < outroMax.Y) && (meuMax.Y > outroMin.Y));
            }
            else
            {
                // Se um dos elementos tiver rodado usa o angulo
                //Usar o maior ratio de imagem e raio mais proximo
                // raiz quadrada de (1/2)*X ~ 0.71f*X

                float raio1 = 0.71f * MathHelper.Max(Tamanho.X, Tamanho.Y);
                float raio2 = 0.71f * MathHelper.Max(outraTextura.Tamanho.X, outraTextura.Tamanho.Y);
                return ((outraTextura.Posicao - Posicao).Length() < (raio1 + raio2)); 
            }
        }
        public void boundToCamera()
        {
            Vector2 minCamera = Camera.posicaoCantoInferiorEsq;
            Vector2 maxCamera = Camera.posicaoCantoSuperiorDir;

            Camera.EstadoColisaoCamera estado = Camera.ColisaoComJanela(this);
            switch(estado)
            {
                case Camera.EstadoColisaoCamera.ColisaoInferior:
                    posicaoI.Y = minCamera.Y + (tamanhoI.Y / 2f);
                    break;
                case Camera.EstadoColisaoCamera.ColisaoSuperior:
                    posicaoI.Y = minCamera.Y - (tamanhoI.Y / 2f);
                    break;
                case Camera.EstadoColisaoCamera.ColisaoEsq:
                    posicaoI.X = minCamera.X + (tamanhoI.X / 2f);
                    break;
                case Camera.EstadoColisaoCamera.ColisaoDir:
                    posicaoI.X = minCamera.X - (tamanhoI.X / 2f);
                    break;
            }
        }
        virtual public void Draw()
        {
            //Area de jogo.
            Rectangle areaDEjogo = Camera.ConvercaoParaPixeisRectangulo(Posicao, Tamanho);
            //Ponto de pendulo onde se exerce a rotacao
            Vector2 og = new Vector2(assetVisual.Width / 2, assetVisual.Height / 2);
            //Draw das texturas
            Game1.spriteBatch.Draw(assetVisual, areaDEjogo, null, Color.White, rotacaoI, og, SpriteEffects.None, 0f);
            if (null != Texto)
                TextoCust.PrintTextoLocalizacao(Posicao, Texto, CorTexto);
            //Print numa localizacao expecifica.
        }
    }
}
