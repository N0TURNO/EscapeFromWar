using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace EscapeFromWar.GraphicsSupport
{
    public class ObjectoDeJogo: Texturas
    {
        //Direccao inicial é o vector(0,1) quando o angulo de rotacao é zero
        protected Vector2 direcaoFrontalI = Vector2.UnitY;

        //Direccao e valor da velocidade do objecto de jogo
        protected Vector2 direccaoVelocidade;//Caso nao seja nula é sempre o vector normal
        protected float velocidade;

        //Construtor do objecto de jogo
        protected void InicializaObejecto()
        {
            direccaoVelocidade = Vector2.Zero;
            velocidade = 0f;
        }

        public ObjectoDeJogo(String nomeSprite, Vector2 posicao, Vector2 tamanho, String texto=null)
            :base(nomeSprite, posicao, tamanho, texto=null)
        {
            InicializaObejecto();
        }

        virtual public void Update()
        {
            posicaoI += (direccaoVelocidade * velocidade);
        }

        public Vector2 DirecaoFrontalInicial
        {
            get { return direcaoFrontalI; }
            set
            {
                float tamanhoVector = value.Length();
                if (tamanhoVector > float.Epsilon)
                    direcaoFrontalI = value / tamanhoVector;
                else
                    direcaoFrontalI = Vector2.UnitY;
            }
        }

        public Vector2 DireccaoFrontal
        {
            get
            {
                return MostraVector.ModificarPeloAng(direcaoFrontalI, Rotacao);
            }
            set
            {
                float tamanhoVector = value.Length();
                if(tamanhoVector>float.Epsilon)
                {
                    value *= (1f / tamanhoVector);
                    double theta = Math.Atan2(value.Y, value.X);
                    rotacaoI = -(float)(theta - Math.Atan2(direcaoFrontalI.Y, direcaoFrontalI.X));
                }
            }
        }
        public Vector2 VelocidadeVector
        {
            get { return direccaoVelocidade * velocidade; }
            set
            {
                velocidade = value.Length();
                if (velocidade > float.Epsilon)
                    direccaoVelocidade = value / velocidade;
                else
                    direccaoVelocidade = Vector2.Zero;
            }
        }
        public float Velocidade
        {
            get { return velocidade; }
            set { velocidade = value; }
        }

        public Vector2 DirecaoVelocidade
        {
            get { return direccaoVelocidade; }
            set
            {
                float tamanho = value.Length();
                if (tamanho > float.Epsilon)
                {
                    direccaoVelocidade = value / tamanho;
                }
                else
                    direccaoVelocidade = Vector2.Zero;
            }
        }

        public bool ObjectivoVisivelDentroCamera()
        {
            Camera.EstadoColisaoCamera estado = Camera.ColisaoComJanela(this);
            return (estado == Camera.EstadoColisaoCamera.DentroJanela);
        }
    }

}
