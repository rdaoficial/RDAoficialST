using System.Windows;

namespace RDAoficialST
{
    public partial class UpdateWindow : Window
    {
        public bool Atualizar { get; private set; }

        public UpdateWindow(string versaoAtual, string novaVersao)
        {
            InitializeComponent();

            TxtVersoes.Text =
                $"Versão atual: {versaoAtual}\n\n" +
                $"Nova versão: {novaVersao}";

            BtnAtualizar.Click += BtnAtualizar_Click;
        }

        private void BtnAtualizar_Click(object sender, RoutedEventArgs e)
        {
            Atualizar = true;
            Close();
        }

        private void BtnDepois_Click(object sender, RoutedEventArgs e)
        {
            Atualizar = false;
            Close();
        }
    }
}