using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using EscapeFromWar.GraphicsSupport;

namespace EscapeFromWar
{
    public class EstadodeJogo
    {
        public enum Ecras
        {
            Menu, Jogar, GameOver
        }
        private Ecras EcraAtual;
        private Texturas EcraMenu;
        private Texturas EcraGameOver;
        private Heroi player;
        TexturasSprite player2;

        const int rapidezMudancaSprite = 10;
        TexturasSprite player1;

        const int numEnimigos = 2;
        Texturas[] inimigos;
        Texturas background, chao, condutas, redes, teto;

        // Imagem de colisao
        Texturas tocaPlayer;//imagem que aparece no ultimo local de colisao
        bool colisaoPixel;//Se o player embate com outra textura
        bool colisaoJanela;//Se o player embate com os limites da janela

        public EstadodeJogo()
        {
            EcraAtual = Ecras.Menu;
            //AudioSupport.Background("menusound", 0.5f);
            InicializarMenu();
            AudioSupport.Background("trailer ", 0.5f);
       
        }

        //Inicializar Menu
        public void InicializarMenu()
        {
            //Calcula o centro do ecrã
            float centroX = (Camera.posicaoCantoSuperiorDir.X - Camera.Largura) / 2;
            float centroY = (Camera.posicaoCantoSuperiorDir.Y - Camera.Altura) / 2;

            //Definições do ecrã
            //EcraMenu = new Texturas("menu", new Vector2(centroX, centroY), new Vector2(Camera.Largura, Camera.Altura), null);
            EcraMenu = new Texturas("MenuF", new Vector2(50, 30), new Vector2(100, 75), null);
            //Mensagem a aparecer
            String msg = "";
            EcraMenu.Texto = msg;
            EcraMenu.CorTexto = Color.White;
        }

        public void InicializarJogar()
        {
            //Cenário
            background = new Texturas("ColisoesF", new Vector2(50, 30), new Vector2(490, 490));
            chao = new Texturas("ChaoF", new Vector2(50, 30), new Vector2(490, 490));
            condutas = new Texturas("Condutas", new Vector2(50, 30), new Vector2(490, 490));
            redes = new Texturas("RedesAndExtras", new Vector2(50, 30), new Vector2(490, 490));
            teto = new Texturas("Teto", new Vector2(50, 30), new Vector2(490, 490));

            //Inimigos
            inimigos = new InimigoPatrulha[numEnimigos];
            inimigos[0] = new InimigoPatrulha("p", new Vector2(7, -180), new Vector2(5, 5),4, 2, 0 );
            inimigos[1] = new InimigoPatrulha("p", new Vector2(17, -180), new Vector2(5, 5), 4, 2, 0);

           tocaPlayer = new Texturas("a", new Vector2(0, 0), new Vector2(20, 20));
            colisaoPixel = false;
            colisaoJanela = false;
            //player2 = new TexturasSprite("St", new Vector2(7, -185), new Vector2(5, 5), 2, 4, 0);
            //player2.DefineAnimacao(0, 0, 0, 2, 10);
            player = new Heroi(new Vector2(7, -185));

            //player.DefineAnimacao(0, 0, 1, 2, 5);
        }


        //Incializar GameOver 
        public void InicializarEcraGameOver()
        {
            //calcular centro do ecrã
            float centroX = Camera.posicaoCantoSuperiorDir.X - Camera.Largura / 2;
            float centroY = Camera.posicaoCantoSuperiorDir.Y - Camera.Altura / 2;

            EcraGameOver = new Texturas("GameOver", new Vector2(50, 30), new Vector2(100, 80), null);
            String msg = "";
            EcraGameOver.Texto = msg;
            EcraGameOver.CorTexto = Color.Red;
        }

        public void UpdateJogo(GameTime gameTime)
        {

            switch (EcraAtual)
            {
                case Ecras.Menu:
                    UpdateMenu();
                    
                    break;
                case Ecras.Jogar:
                    UpdateGamePlay(gameTime);
                    
                    break;
                case Ecras.GameOver:
                    UpdateEcraGameOver();
                    break;
            }          
        }

        public void UpdateGamePlay(GameTime gameTime)
        {

            //Utiliza o input 
            Vector2 playerMoveDelta = InputWrapper.ThumbSticks.Left;
            player.Posicao += playerMoveDelta;
            //AudioSupport.PlaySom("walk fast");

            player.Update();

            UpdateColisoes();

            //Bounce back
            if (colisaoPixel)
                player.Posicao -= playerMoveDelta;
            PlayerMoveCamera();
            UserControlUpdate();

            //if (heroiT.Morto())
            //{
            //    EcraAtual = Ecras.GameOver;
            //    //AudioSupport.PlayACue("Break");
            //    InicializarEcraGameOver();
            //    return;
            //}

            bool hdisparar = (InputWrapper.Buttons.Y == ButtonState.Pressed);
            //h.Update(gameTime, playerMoveDelta, hdisparar);

        }

        public void UpdateMenu()
        {
            if(InputWrapper.Buttons.A==ButtonState.Pressed)
            {
                EcraMenu = null;
                EcraAtual = Ecras.Jogar;
                InicializarJogar();
                AudioSupport.PlaySom("som-bot¦o");
                AudioSupport.Background("exterior_sound", 0.5f);
            }
        }

        public void UpdateEcraGameOver()
        {
            if(InputWrapper.Buttons.A==ButtonState.Pressed)
            {
                EcraGameOver = null;
                EcraAtual = Ecras.Jogar;
                InicializarJogar();
            }
        }

        private void UpdateColisoes()
        {
            Vector2 pixelPosicaoColisao = Vector2.Zero;

            #region Colisao com background
            colisaoJanela = player.ToqueTextures(background);
            colisaoPixel = colisaoJanela;

            if (colisaoJanela)
            {
                colisaoPixel = player.PixeisTocaram(background, out pixelPosicaoColisao);
                if (colisaoPixel)
                {
                    tocaPlayer.Posicao = pixelPosicaoColisao;
                }
            }

            #endregion

            #region Colisao com os inimigos
            int i = 0;
            while((!colisaoPixel)&&(i<numEnimigos))
            {
                colisaoJanela = inimigos[i].ToqueTextures(player);
                colisaoPixel = colisaoJanela;
                if(colisaoJanela)
                {
                   colisaoPixel= inimigos[i].PixeisTocaram(player, out pixelPosicaoColisao);
                    if (colisaoPixel)
                        tocaPlayer.Posicao = pixelPosicaoColisao;
                }
                i++;
            }

            #endregion
        }
        private void UserControlUpdate()
        {
            #region Rotacao do jogador
            if (InputWrapper.Buttons.X == ButtonState.Pressed)
                player.Rotacao += MathHelper.ToRadians(1);
            if (InputWrapper.Buttons.Y == ButtonState.Pressed)
                player.Rotacao += MathHelper.ToRadians(-1);
            #endregion

            #region Update Sprites
            if(InputWrapper.ThumbSticks.Left.X==0)
            {
                player.SpriteColunaFinal = 0;//Para animacao
            }
            else
            {
                float movX = InputWrapper.ThumbSticks.Left.X;
                player.SpriteColunaFinal = 3;
                if(movX<0)
                {
                    player.SpriteLinhaInicial = 1;
                    player.SpriteLinhaFinal = 1;
                    movX *= -1f;
                }
                else
                {
                    player.SpriteLinhaInicial=0;
                    player.SpriteLinhaFinal = 0;

                }
                player.SpriteTimerAnimacao = (int)((1f - movX) * rapidezMudancaSprite);
                //AudioSupport.PlaySom("wla")
            }
            #endregion

            #region Controlo camera
            //Zoom
            if (InputWrapper.Buttons.A == ButtonState.Pressed)
                Camera.ZoomCamera(5);
            if (InputWrapper.Buttons.B == ButtonState.Pressed)
                Camera.ZoomCamera(-5);

            //MOve camera com o analogico direito
           // Camera.MoverCamera(InputWrapper.ThumbSticks.Right);
            
            #endregion

        }

        private void PlayerMoveCamera()
        {
            Camera.EstadoColisaoCamera estado = Camera.ColisaoComJanela(player);
            Vector2 delta = Vector2.Zero;
            Vector2 cameraAngInf_Esq = Camera.posicaoCantoInferiorEsq;
            Vector2 cameraAngSup_Dir = Camera.posicaoCantoSuperiorDir;
            const float velocidadeCamera = 0.5f;
            float distPlayer_Bound = player.Largura * 15f;
            switch(estado)
            {
                case Camera.EstadoColisaoCamera.ColisaoInferior:
                    delta.Y = (player.Posicao.Y - distPlayer_Bound - cameraAngInf_Esq.Y) * velocidadeCamera;
                    break;
                case Camera.EstadoColisaoCamera.ColisaoSuperior:
                    delta.Y = (player.Posicao.Y + distPlayer_Bound - cameraAngSup_Dir.Y) * velocidadeCamera;
                    break;
                case Camera.EstadoColisaoCamera.ColisaoEsq:
                    delta.X = (player.Posicao.X - distPlayer_Bound - cameraAngInf_Esq.X) * velocidadeCamera;
                    break;
                case Camera.EstadoColisaoCamera.ColisaoDir:
                    delta.X = (player.Posicao.X + distPlayer_Bound - cameraAngSup_Dir.X) * velocidadeCamera;
                    break;

            }
            Camera.MoverCamera(delta);
        }
        public void DrawJogo()
        {
            switch(EcraAtual)
            {
                case Ecras.Menu:
                  
                    if (EcraMenu != null)
                        EcraMenu.Draw();
                    break;
                case Ecras.Jogar:
                    chao.Draw();
                    redes.Draw();
                    background.Draw();
                    condutas.Draw();
                    //foreach (var p in inimigos)
                    //    p.Draw();
                    inimigos[0].Draw();
                    inimigos[1].Draw();
                    player.Draw();
                    
                    teto.Draw();
                    TextoCust.PrintTexto("", null);

                    break;
                case Ecras.GameOver:
                    if (EcraGameOver != null)
                        EcraGameOver.Draw();
                    break;
            }                    
        }
    }  
}
