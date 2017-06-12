using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace EscapeFromWar.GraphicsSupport
{
    public class RodaObjecto : ObjectoDeJogo
    {
        public RodaObjecto(String imagem, Vector2 centro, float raio)
            : base(imagem, centro, new Vector2(raio * 2f, raio * 2f))
        {

        }

        public float Raio
        {
            get { return tamanhoI.X / 2f; }
            set
            {
                tamanhoI.X = 2f * value;
                tamanhoI.Y = tamanhoI.X;
            }
        }

        public override void Update()
        {
            //Movimento dos obejectos pelo valor da velocidade
            base.Update();

            Vector2 vectorVelocidade = VelocidadeVector;
            vectorVelocidade.Y -= Game1.gravidade;
            VelocidadeVector = vectorVelocidade;

            //Agora roda o objecto de acordo com o valor da velocidade do eixo dos xx
            float deslocamentoAngular = (vectorVelocidade.X / Raio);
            //Isto assume que o objecto esta em cima de superficies
            if (vectorVelocidade.X > 0)
                rotacaoI += deslocamentoAngular;
            else
                rotacaoI -= deslocamentoAngular;

        }

    }
}
