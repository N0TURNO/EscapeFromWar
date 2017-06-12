using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EscapeFromWar.GraphicsSupport;

namespace EscapeFromWar
{
    public class TexturasSprite : ObjectoDeJogo

    {
        private int numeroColuna, numeroLinha, numeroEspacos;
        //numero de colunas, linhas e espacos

        private int SpriteILargura, SpriteIAltura;
        //Dimensao de imagem

        #region Animacao
        private int timerProximaAnimacao;
        //Tempo ate mudar de imagem na animacao

        private int timerActualAnimacao;
        //Tempo desde que pegou nesta imagem

        private int linhaActual, colunaActual;
        //linha e coluna actual da imagem

        private int linhaInicial, linhaFinal;
        private int colunaInicial, colunaFinal;
        //Inicio e fim da animacao mas é necessario verificar se o inicio é superior ao fim
        #endregion

        public TexturasSprite(String imagem, Vector2 posicao, Vector2 tamanho, int numLinhas, int numColunas, int espacosImagens)
            : base(imagem, posicao, tamanho)
        {
            numeroLinha = numLinhas;
            numeroColuna = numColunas;
            numeroEspacos = espacosImagens;

            SpriteILargura = assetVisual.Width / numeroLinha;
            SpriteIAltura = assetVisual.Height / numeroColuna;

            //Inicializacao por animacao sempre a mostra o topo esquerdo da imagem
            timerProximaAnimacao = 1;
            timerActualAnimacao = 0;
            linhaActual = 0;
            colunaActual = 0;
            linhaInicial = colunaInicial = linhaFinal = colunaFinal = 0;
        }
        public int SpriteLinhaActual { get { return linhaActual; } set { linhaActual = value; } }
        public int SpriteColunaActual { get { return colunaActual; } set { colunaActual = value; } }


        public int SpriteLinhaInicial { get { return linhaInicial; } set { linhaInicial = value; linhaActual = value; } }
        public int SpriteLinhaFinal { get { return linhaFinal; } set { linhaFinal = value; } }
        public int SpriteColunaInicial { get { return colunaInicial; } set { colunaInicial = value; colunaActual = value; } }
        public int SpriteColunaFinal { get { return colunaFinal; } set { colunaFinal = value; } }
        public int SpriteTimerAnimacao { get { return timerProximaAnimacao; } set { timerProximaAnimacao = value; } }

        public void DefineAnimacao(int linhainicial, int colunainicial, int linhafinal, int colunafinal, int timer)
        {
            timerProximaAnimacao = timer;
            linhaInicial = linhainicial;
            colunaInicial = colunainicial;
            linhaFinal = linhafinal;
            colunaFinal = colunafinal;

            //Inicializar animacao
            linhaActual = linhaInicial;
            colunaActual = colunaInicial;
            timerActualAnimacao = 0;


        }
        public override void Update()
        {
            base.Update();

            //Update estado Sprite
            timerActualAnimacao++;

            if (timerActualAnimacao > timerProximaAnimacao)
            {
                timerActualAnimacao = 0;//Comeca a proxima imagem de sprite

                colunaActual++;//Coluna a seguir
                if (colunaActual > colunaFinal)
                {
                    //Reinicia a animation
                    colunaActual = colunaInicial;
                    colunaActual++;

                    if (linhaActual > linhaFinal)
                        linhaActual = linhaInicial;
                }

            }
        }
        public override void Draw()
        {
            //localizacao e tamanho da area a sert desenhada em pixel space
            Rectangle areaTextura = Camera.ConvercaoParaPixeisRectangulo(Posicao, Tamanho);

            int topoImagem = linhaActual * SpriteILargura;
            int esqImagem = colunaActual * SpriteIAltura;

            //Origem da rotacao (ferencia de rotacao)
            Vector2 origem = new Vector2(SpriteILargura / 2, SpriteIAltura / 2);

            //define a area e a localizacao a printar da SpriteSheet para a area de jogoo
            Rectangle areaSprite = new Rectangle(esqImagem + numeroEspacos, topoImagem + numeroEspacos, SpriteILargura
                , SpriteIAltura);

           

            //Print das texturas
            Game1.spriteBatch.Draw(assetVisual, areaTextura, areaSprite, tintImage, rotacaoI, origem, SpriteEffects.None, 0f);

            if (null != Texto)
                TextoCust.PrintTextoLocalizacao(Posicao, Texto
                    , CorTexto);
        }

        #region Override para suportar a colisao pixeis a pixel
        protected override int SpritePixeltopoSup
        { get { return linhaActual * SpriteIAltura; } }
        protected override int SpritePixelbaixoEsq
        { get { return colunaActual * SpriteILargura; } }
        protected override int SpriteLargura
        { get { return SpriteILargura; } }
        protected override int SpriteAltura
        {get  { return SpriteIAltura; } }
        #endregion
    }
}
            
        
    

