using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EscapeFromWar.GraphicsSupport
{
    public class InimigoPatrulha : TexturasSprite
    {
        protected enum EstadoPatrulha
        {
            Patrulha, Perseguicao, Stunned, Morto
        }
        protected enum TipoPatrulha
        {
            Aleatoria, MovHorizontal, MovVertical
        }

        private const float velociadePatrulha = 0.2f;
        private const float raioPersecpcao = 20f;//Para outros inimigos
        private const float raioParaPerseguir = 40f;//Para iniciar o estado de pereguicao
        private const int timerEstado = 60 * 5;//Sendo que os Updates sao de 60 por segundo demora 5 segundos a mudar de estado
        private const int tempoStunned = timerEstado / 2;
        private const float velocidadePerseguicao = 0.3f;
        protected const float larguraInimigo = 10f;
        protected const int tamanhoIinimigo = 1;
        private const float BordaRaio = 0.55f;

        private Color corTinge = Color.Black;
        private Color corTingePerseguicao = Color.OrangeRed;
        private Color corTingeStunned = Color.LightCyan;

        private Vector2 posicaoAlvo;
        private EstadoPatrulha estadoActual;
        protected TipoPatrulha tipoPatrulha;
        protected TipoEnimigo tipoActual;
        protected bool permiteRotacao;

        private int timerEmEstado;

        private bool destroiFlag;

        public bool DestroiFlag
        { get { return destroiFlag; } }

        protected int tamanhoIni;
        public int TamanhoIni
        {
            get { return tamanhoIni; }
            set
            {
                tamanhoIni = value;
                this.Tamanho = new Vector2(tamanhoIni * larguraInimigo + larguraInimigo,
                    tamanhoIni * larguraInimigo + larguraInimigo);
                
            }
        }
    
        
        public InimigoPatrulha(String imagem, Vector2 posicao, Vector2 tamanho, int contagemLinha, int contagemColuna, int espacosImagens) :
            base(imagem, posicao, tamanho, contagemLinha, contagemColuna, espacosImagens)
        {
            //causa um update no estado
            posicaoAlvo = Posicao = Vector2.Zero;
            VelocidadeVector = Vector2.UnitY;
            tintImage = corTinge;
            tipoPatrulha = TipoPatrulha.Aleatoria;
            Posicao = PosicaoAleatoria(true);
            destroiFlag = false;
            DefineAnimacao(0, 0, 0, 3, 10);
            TamanhoIni = tamanhoIinimigo;
            tipoActual = TipoEnimigo.SoldadoExterno;
            //DEPOIS adicionaar o externo


            
        }

        public bool UpdatePatrulha(Heroi hero, out Vector2 posicaoApanhado)
        {

            bool apanhado = false;
            posicaoApanhado = Vector2.Zero;

            timerEmEstado--;

            //Comum a todos os estados 
            if(estadoActual!=EstadoPatrulha.Stunned)
            {
                base.Update();
                Vector2 paraHeroi = hero.Posicao - Posicao;
                paraHeroi.Normalize();
                Vector2 paraAlvo = posicaoAlvo - Posicao;
                float distanciaAlvo = paraAlvo.Length();
                paraAlvo.Normalize();//Diferente do codigo
                CalculoNovaDireccao(paraAlvo, paraHeroi);

                switch(estadoActual)
                {
                    case EstadoPatrulha.Patrulha:
                        UpdatePatrulhaEstado(hero, distanciaAlvo);
                        break;
                    case EstadoPatrulha.Perseguicao:
                        apanhado = UpdateEstadoPerseguicao(hero, distanciaAlvo, out posicaoApanhado);
                        break;
                }
                
            }
            else
            {
                UpdateEstadoStunned(hero);
            }
            return apanhado;
        }

        private bool UpdateEstadoPerseguicao(Heroi hero, float distaciaHeroi, out Vector2 posicao)
        {
            bool apanhado = false;
            apanhado = PixeisTocaram(hero, out posicao);//Podese usar aqui o metode de detetecccao do triangulo
            posicaoAlvo = hero.Posicao;

            if (apanhado)
            {
                switch (tipoActual)
                {
                    case TipoEnimigo.SoldadoInterno:
                        hero.AjustaVida(-1);
                        this.TamanhoIni--;
                        this.destroiFlag = true;
                        break;
                    case TipoEnimigo.SoldadoExterno:
                        hero.AjustaVida(-1);
                        this.TamanhoIni--;
                        this.destroiFlag = true;
                        break;
                        //codigo alterado da fonte
                }
            }
            else if (timerEmEstado < 0)
                ProximoAlvo();//quando acaba o tempo passa para o proximo

            return apanhado;
        }

        private void DetectaInimigo(ObjectoDeJogo heroi)
        {
            Vector2 paraHeroi = heroi.Posicao - Posicao;
            if(paraHeroi.Length()<raioParaPerseguir)
            {
                DefineParaEstadoPerseguicao(heroi);
            }
        }
       public void  DefineParaEstadoPerseguicao(ObjectoDeJogo heroi)
        {
            timerEmEstado = (int)(timerEstado * 1.5f);
            Velocidade = velociadePatrulha;
            estadoActual = EstadoPatrulha.Perseguicao;
            posicaoAlvo = heroi.Posicao;
            tintImage = corTingePerseguicao;
        }
            private void CalculaNovoTimerVelocidade_Reset()
        {
            Velocidade = velociadePatrulha * (0.8f + (float)(0.4 * Game1.numAleatorios.NextDouble()));
            timerEmEstado=(int)(timerEstado*(0.8f+(float)(0.6*Game1.numAleatorios.NextDouble())));
        }

        public void DefineParaEstadoStunned()
        {
            tintImage = corTingeStunned;
            timerEmEstado = tempoStunned;
            estadoActual = EstadoPatrulha.Stunned;
            AudioSupport.PlaySom("Stunned");
        }
        private void UpdateEstadoStunned(Heroi hero)
        {
            if (timerEmEstado < 0)
                DefineParaEstadoPerseguicao(hero);
        }
        private void CalculoNovaDireccao(Vector2 paraAlvo, Vector2 paraHeroi)
        {
            if(permiteRotacao)
            {
                //Verifica se é necessario continuar a ajustar a direcçao
                double costheta = Vector2.Dot(paraAlvo, DireccaoFrontal);
                float theta = (float)Math.Acos(costheta);//Calcula novo angulo
                if(theta>float.Epsilon)
                {
                    Vector3 direccaoFront3 = new Vector3(DireccaoFrontal, 0f);
                    Vector3 paraAlvo3 = new Vector3(paraAlvo, 0f);
                    Vector3 eixoZ = Vector3.Cross(direccaoFront3, paraAlvo3);
                    Rotacao -= Math.Sign(eixoZ.Z) * 0.03f * theta;//roda a cada 5%
                    DirecaoVelocidade = DireccaoFrontal;

                }
                else
                {
                    DirecaoVelocidade = paraAlvo;
                    if (DirecaoVelocidade.X > 0)
                        SpriteLinhaActual = 1;
                    else if (DirecaoVelocidade.X < 0)
                        SpriteColunaActual = 0;
                }
            }
        }

        private void ProximoAlvo()
        {
            timerEmEstado = timerEstado;
            estadoActual = EstadoPatrulha.Patrulha;
            tintImage = corTinge;
            double InicioEstado = Game1.numAleatorios.NextDouble();
            if (InicioEstado < 0.25)
                posicaoAlvo = PosiçaoAleatoriaInfDir();
            else if (InicioEstado < 0.5)
                posicaoAlvo = PosiçaoAleatoriaSupDir();
            else if (InicioEstado < 0.75)
                posicaoAlvo = PosiçaoAleatoriaSupEsq();
            else
                posicaoAlvo = PosiçaoAleatoriaInfEsq();

            CalculaNovoTimerVelocidade_Reset();
        }

        private Vector2 GeraInimigosForaX(double xFora, double yFora)
        {
            Vector2 max = new Vector2(PosicaoX + Camera.Largura / 2, Camera.posicaoCantoSuperiorEsq.Y);
            Vector2 min = new Vector2(PosicaoX - Camera.Largura / 2, Camera.posicaoCantoInferiorEsq.Y);
            float x = min.X + tamanhoI.X * (float)(xFora+(BordaRaio* Game1.numAleatorios.NextDouble()));
            float y = max.Y + tamanhoI.Y * (float)(yFora+ (BordaRaio * Game1.numAleatorios.NextDouble()));
            return new Vector2(x, y);

        }
        const float minOffset = 0.05f;
        private Vector2 PosiçaoAleatoriaInfDir()
        {
            return GeraInimigosForaX(0.5, minOffset);
        }
        private Vector2 PosiçaoAleatoriaSupDir()
        {
            return GeraInimigosForaX(0.5, 0.5);
        }
        private Vector2 PosiçaoAleatoriaInfEsq()
        {
            return GeraInimigosForaX(minOffset, minOffset);
        }
        private Vector2 PosiçaoAleatoriaSupEsq()
        {
            return GeraInimigosForaX(minOffset, 0.5);
        }
        

        public Vector2 PosicaoAleatoria(bool foraCamera)
        {
            Vector2 posicao;
            float posX = (float)Game1.numAleatorios.NextDouble() * Camera.Largura * 0.80f + Camera.Largura * 0.10f;
            float posY = (float)Game1.numAleatorios.NextDouble() * Camera.Altura* 0.80f + Camera.Altura * 0.10f;

            if (foraCamera)
                posX += Camera.posicaoCantoSuperiorDir.X;

            posicao = new Vector2(posX, posY);
            return posicao;
        }
        private void GeraInimigo_UpDown()
        {
            timerEmEstado = timerEstado;
            estadoActual = EstadoPatrulha.Patrulha;
            tintImage = corTinge;
            float posY;
            //Usa os limites da camera
            float distanciaLIMTopo = Camera.posicaoCantoSuperiorEsq.Y - PosicaoY;
            float distanciaLIMInf = PosicaoY - Camera.posicaoCantoInferiorEsq.Y;
            if(distanciaLIMTopo>distanciaLIMInf)//Caso a distancia ate ao limite superior fosse maior entao produz um movimwnto superior caso contrario um movimento inferior
            {
                posY = (float)Game1.numAleatorios.NextDouble() * distanciaLIMTopo / 2 * 0.8f + PosicaoY + distanciaLIMTopo / 2;
            }
            else
            {
                posY = (float)Game1.numAleatorios.NextDouble() * -distanciaLIMInf / 2 * 0.8f + PosicaoY + distanciaLIMInf / 2;
            }
            posicaoAlvo = new Vector2(PosicaoX, posY);
            CalculaNovoTimerVelocidade_Reset();
        }
        private void GeraInimigo_LefRight()
        {
            timerEmEstado = timerEstado;
            estadoActual = EstadoPatrulha.Patrulha;
            tintImage = corTinge;
            float posX;
            
            if(VelocidadeVector.X<=0)
            {
                posX = (float)Game1.numAleatorios.NextDouble() * Camera.Largura / 2 + PosicaoX;
            }
            else
            {
                posX = (float)Game1.numAleatorios.NextDouble() * -Camera.Largura / 2 + PosicaoX;
            }
            CalculaNovoTimerVelocidade_Reset();
        }


        public void UpdatePatrulhaEstado(ObjectoDeJogo hero, float distanciaAlvo)
        {
            if(timerEmEstado<0||distanciaAlvo<raioPersecpcao)
            {
                switch(tipoPatrulha)
                {
                    case TipoPatrulha.Aleatoria:
                        ProximoAlvo();
                        break;
                    case TipoPatrulha.MovHorizontal:
                        GeraInimigo_LefRight();
                        break;
                    case TipoPatrulha.MovVertical:
                        GeraInimigo_UpDown();
                        break;

                }
            }
            

        }
    }
}
