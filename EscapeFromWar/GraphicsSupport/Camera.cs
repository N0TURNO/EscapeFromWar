using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace EscapeFromWar.GraphicsSupport
{
   static public class Camera
    {
        static private Vector2 origem = Vector2.Zero; // Estabelece a origem da area de jogo com o vector nulo.
        static private float largura = 100f; // Largura da area de jogo.
        static private float altura = -1f; // Altura da janela de jogo. 
        static private float ratio = -1f; // Razao entre area de jogo e janela de jogo.
        static public float Largura { get { return largura; } }
        static public float Altura { get { return altura; } }


        static private float RazaoAreaJogo()
        {
            if(ratio<0f)
            {
                ratio = (float)Game1.graphics.PreferredBackBufferWidth / largura; // Razao da janela.
                altura = largura * (float)Game1.graphics.PreferredBackBufferHeight / (float)Game1.graphics.PreferredBackBufferWidth; // Altura é definida pela razao entre larguras(dada e adquirida) vezes a altura adquirida.
            }
            return ratio;
        }

        static public void DefineCamera(Vector2 origemP, float larguraP)
        {

                origem = origemP;
                largura = larguraP;
                RazaoAreaJogo();
            
        }

        static public void MoverCamera(Vector2 delta)
        {
            origem += delta;
        }

        static public void ZoomCamera(float deltaX)
        {
            float ogLargura = largura; // Guarda altura original da janela.
            float ogAltura = altura; // Guarda altura original da janela.
           
            largura =largura + deltaX; // Modifica o valor da largura.
            ratio = -1f;
            RazaoAreaJogo(); // Chamar o ratio da area do jogo.
            
            float dX = 0.5f * (largura - ogLargura);
            float dY = 0.5f * (altura - ogAltura);
            origem -= new Vector2(dX, dY);
        }

        static public void ConvercaoParaPixeisPosicao(Vector2 posicaoCamera, out int x, out int y)
        {
            float ratio = RazaoAreaJogo();

            x = (int)(((posicaoCamera.X - origem.X) * ratio) + 0.5f);
            y = (int)(((posicaoCamera.Y - origem.Y) * ratio) + 0.5f);
            y = Game1.graphics.PreferredBackBufferHeight - y;

            
        }
        static public Rectangle ConvercaoParaPixeisRectangulo(Vector2 posicao, Vector2 tamanho)
            {
            float ratio = RazaoAreaJogo();
            // Converter a janela de jogo para espaço de pixeis.
            int largura = (int)((tamanho.X * ratio) + 0.5f);
            int altura = (int)((tamanho.Y * ratio) + 0.5f);
            // Converte a posiçao da camera para espaço de pixeis.
            int x, y;
            ConvercaoParaPixeisPosicao(posicao, out  x, out  y);
            return new Rectangle(x, y, largura, altura);
        }

        //Limites da Camera(Apenas são necessarias as coordenadas de origem e do limite suprior direito).
        static public Vector2 posicaoCantoInferiorEsq { get { return origem; } }
        static public Vector2 posicaoCantoSuperiorDir { get { return origem + new Vector2(largura, altura); } }
        static public Vector2 posicaoCantoSuperiorEsq { get { return origem + new Vector2(0, altura); } }
        static public Vector2 posicaoCantoInferiorDir { get { return origem + new Vector2(largura, 0); } }

        // Enumerador que suporta as quatro posiveis colisoes com os limites de camera.
        public enum EstadoColisaoCamera
        {
            ColisaoSuperior = 0, ColisaoInferior, ColisaoEsq, ColisaoDir, DentroJanela
        };

        static public EstadoColisaoCamera ColisaoComJanela(Texturas prim)
        {
            Vector2 min = posicaoCantoInferiorEsq;
            Vector2 max = posicaoCantoSuperiorDir;

            if (prim.LimiteMaxx.Y > max.Y)
                return EstadoColisaoCamera.ColisaoSuperior;
            if (prim.LimiteMin.X < min.X)
                return EstadoColisaoCamera.ColisaoEsq;
            if (prim.LimiteMaxx.X > max.X)
                return EstadoColisaoCamera.ColisaoDir;
            if (prim.LimiteMin.Y < min.Y)
                return EstadoColisaoCamera.ColisaoInferior;

            return EstadoColisaoCamera.DentroJanela;

            
        }

        static public Vector2 PosicaoAleatoria()
        {

            float rangeX = 0.8f * largura;
            float offSetX = 0.1f * largura;
            float rangeY = 0.8f * altura;
            float offSetY = 0.1f * altura;

            float x = (float)(Game1.numAleatorios.NextDouble()) * rangeX + offSetX + origem.X;
            float y = (float)(Game1.numAleatorios.NextDouble()) * rangeY + offSetY + origem.Y;

            return new Vector2(x, y);

        }
    }
}
