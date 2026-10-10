using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using Forms = System.Windows.Forms;
using Path = System.IO.Path;

namespace RDAoficialST
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            CriarParticulas();
            AnimarFundo();
            AnimarTitulo();
            AnimarLogo();
        }





        private void BtnInstalarPacote_Click(object sender, RoutedEventArgs e)
        {
            const string destino =
                @"C:\Program Files (x86)\Steam\config\stplug-in";

            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();

                var recursos = assembly.GetManifestResourceNames()
                    .Where(n => n.EndsWith(
                        ".lua", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (recursos.Count == 0)
                {
                    System.Windows.MessageBox.Show(
                        "Nenhum arquivo Lua foi encontrado nos recursos incorporados.",
                        "Pacote de jogos",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                Directory.CreateDirectory(destino);

                string raiz = Path.GetFullPath(destino)
                    .TrimEnd(Path.DirectorySeparatorChar) +
                    Path.DirectorySeparatorChar;

                var arquivos = new List<(string Recurso, string Caminho)>();


                const string prefixo = "PacoteJogos.";

                foreach (string recurso in recursos)
                {
                    if (!recurso.StartsWith(
                        prefixo, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    // Remove apenas o prefixo interno do recurso.
                    string relativo = recurso.Substring(prefixo.Length);

                    string caminho = Path.GetFullPath(
                        Path.Combine(destino, relativo));

                    if (!caminho.StartsWith(
                        raiz, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new IOException(
                            "Caminho inválido no recurso: " + recurso);
                    }

                    arquivos.Add((recurso, caminho));
                }


                var conflitos = arquivos
                    .Where(a => File.Exists(a.Caminho))
                    .ToList();

                if (conflitos.Count > 0)
                {
                    var resposta = System.Windows.MessageBox.Show(
                        "Já existem " + conflitos.Count +
                        " Jogos comprados. Deseja comprar-los novamente?",
                        "Confirmar compra",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (resposta != MessageBoxResult.Yes)
                    {
                        TxtStatusPacote.Text = "Compra cancelada.❌";
                        return;
                    }
                }

                int instalados = 0;

                foreach (var arquivo in arquivos)
                {
                    string pasta = Path.GetDirectoryName(arquivo.Caminho);

                    if (!string.IsNullOrEmpty(pasta))
                        Directory.CreateDirectory(pasta);

                    using (Stream entrada =
                        assembly.GetManifestResourceStream(arquivo.Recurso))
                    {
                        if (entrada == null)
                            throw new IOException(
                                "Não foi possível ler: " + arquivo.Recurso);

                        using (FileStream saida = new FileStream(
                            arquivo.Caminho,
                            FileMode.Create,
                            FileAccess.Write,
                            FileShare.None))
                        {
                            entrada.CopyTo(saida);
                        }
                    }

                    instalados++;
                }

                TxtStatusPacote.Text =
                    $"Compra concluída: {instalados} jogos(s).✅";

                ShowToast(
                    "Pacote de jogos",
                    $"{instalados} jogos comprados.✅");
            }
            catch (Exception ex)
            {
                TxtStatusPacote.Text = "Falha na compra.❌";

                System.Windows.MessageBox.Show(
                    ex.Message,
                    "Erro ao comprar pacote ❌",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }






        private void ShowToast(string titulo, string mensagem)
        {
            Border toast = new Border
            {
                Width = 280,
                CornerRadius = new CornerRadius(12),
                Background = new SolidColorBrush(
                    Color.FromRgb(20, 20, 20)),
                BorderBrush = Brushes.Gold,
                BorderThickness = new Thickness(1),
                Padding = new Thickness(15),
                Margin = new Thickness(0, 0, 0, 10),
                Opacity = 0
            };

            StackPanel panel = new StackPanel();

            panel.Children.Add(new TextBlock
            {
                Text = titulo,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Gold,
                FontSize = 14
            });

            panel.Children.Add(new TextBlock
            {
                Text = mensagem,
                Foreground = Brushes.White,
                Margin = new Thickness(0, 5, 0, 0),
                TextWrapping = TextWrapping.Wrap
            });

            toast.Child = panel;

            ToastContainer.Children.Add(toast);

            DoubleAnimation fadeIn =
                new DoubleAnimation(0, 1,
                    TimeSpan.FromMilliseconds(300));

            toast.BeginAnimation(
                UIElement.OpacityProperty,
                fadeIn);

            Task.Delay(3000).ContinueWith(_ =>
            {
                Dispatcher.Invoke(() =>
                {
                    DoubleAnimation fadeOut =
                        new DoubleAnimation(
                            1,
                            0,
                            TimeSpan.FromMilliseconds(400));

                    fadeOut.Completed += (s, e) =>
                    {
                        ToastContainer.Children.Remove(toast);
                    };

                    toast.BeginAnimation(
                        UIElement.OpacityProperty,
                        fadeOut);
                });
            });
        }


        private void TxtSobre_Click(
            object sender,
            System.Windows.Input.MouseButtonEventArgs e)
                {
                    MostrarSobre();
                }

        private void AnimarLogo()
        {
            DoubleAnimation brilho = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(4),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };

            DoubleAnimation zoom = new DoubleAnimation
            {
                From = 1.0,
                To = 1.08,
                Duration = TimeSpan.FromSeconds(4),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };

            LogoGlow.BeginAnimation(UIElement.OpacityProperty, brilho);

            LogoScale.BeginAnimation(
                ScaleTransform.ScaleXProperty,
                zoom);

            LogoScale.BeginAnimation(
                ScaleTransform.ScaleYProperty,
                zoom);
        }

        private void AnimarTitulo()
        {
            DoubleAnimation brilho = new DoubleAnimation
            {
                From = -1.5,
                To = 1.5,
                Duration = TimeSpan.FromSeconds(11),
                RepeatBehavior = RepeatBehavior.Forever
            };

            TituloGradientTransform.BeginAnimation(
                TranslateTransform.XProperty,
                brilho);
        }


        private void AnimarFundo()
        {
            DoubleAnimation zoomX = new DoubleAnimation
            {
                From = 1.0,
                To = 1.05,
                Duration = TimeSpan.FromSeconds(9),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };

            DoubleAnimation zoomY = new DoubleAnimation
            {
                From = 1.0,
                To = 1.05,
                Duration = TimeSpan.FromSeconds(9),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };

            FundoScale.BeginAnimation(ScaleTransform.ScaleXProperty, zoomX);
            FundoScale.BeginAnimation(ScaleTransform.ScaleYProperty, zoomY);
        }

        private readonly Random random = new Random();

        private void CriarParticulas()
        {
            for (int i = 0; i < 150; i++)
            {
                Color[] cores =
                {
                    Color.FromArgb(120,255,230,180),
                    Color.FromArgb(100,255,215,140),
                    Color.FromArgb(90,240,200,120),
                    Color.FromArgb(110,255,240,200)
                };

                Color cor = cores[random.Next(cores.Length)];

                int tamanho = random.Next(2, 4);

                System.Windows.Shapes.Ellipse particula =
                    new System.Windows.Shapes.Ellipse
                {
                    Width = tamanho,
                    Height = tamanho,
                    Fill = new SolidColorBrush(cor)
                };

                Canvas.SetLeft(particula, random.Next(0, 800));
                Canvas.SetTop(particula, random.Next(0, 900));

                ParticleCanvas.Children.Add(particula);

                AnimarParticula(particula);
            }
        }

        private void AnimarParticula(System.Windows.Shapes.Ellipse particula)
        {
            ReiniciarParticula(particula);
        }

        private void ReiniciarParticula(System.Windows.Shapes.Ellipse particula)
        {
            double posX = random.Next(-50, 730);
            double posY = random.Next(650, 850);

            Canvas.SetLeft(particula, posX);
            Canvas.SetTop(particula, posY);

            double destinoX = posX + random.Next(-80, 80);
            double destinoY = -100;

            int duracao = random.Next(15, 30);

            DoubleAnimation moverX = new DoubleAnimation
            {
                From = posX,
                To = destinoX,
                Duration = TimeSpan.FromSeconds(duracao)
            };

            DoubleAnimation moverY = new DoubleAnimation
            {
                From = posY,
                To = destinoY,
                Duration = TimeSpan.FromSeconds(duracao),
                FillBehavior = FillBehavior.Stop
            };

            DoubleAnimation opacidade = new DoubleAnimation
            {
                From = 0,
                To = 0.9,
                Duration = TimeSpan.FromSeconds(duracao / 2.0),
                AutoReverse = true
            };

            moverY.Completed += (s, e) =>
            {
                ReiniciarParticula(particula);
            };

            particula.BeginAnimation(Canvas.LeftProperty, moverX);
            particula.BeginAnimation(Canvas.TopProperty, moverY);
            particula.BeginAnimation(UIElement.OpacityProperty, opacidade);
        }


    


        // ==========================================
        // TUTORIAL RYUUFIX
        // ==========================================
        private void BtnTutorialryuufix_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.youtube.com/watch?v=YvhO-i8UfOU&t=503s",
                UseShellExecute = true
            });
        }


        // ==========================================
        // TUTORIAL ONLINEFIX
        // ==========================================
        private void BtnTutorialonlinefix_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://youtu.be/qe5UCq-_zhE?si=rJuNYC6PdLteBva6",
                UseShellExecute = true
            });
        }



        // ==========================================
        // SITE ONLINEFIX
        // ==========================================

        private void BtnOnlineFix_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://online-fix.me/",
                UseShellExecute = true
            });
        }



        // ==========================================
        // SITE Ryuu Fixes
        // ==========================================

        private void BtnRyuufix_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://generator.ryuu.lol/fixes#",
                UseShellExecute = true
            });
        }


        private void BtnYoutube_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.youtube.com/@rdaoficiall",
                UseShellExecute = true
            });
        }


        // ==========================================
        // BACKUP AUTOMÁTICO
        // ==========================================

        private void BtnBackup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string origem =
                    @"C:\Program Files (x86)\Steam\config\stplug-in";

                string destino =
                    Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                        "stplug-in"
                    );

                if (Directory.Exists(origem))
                {
                    if (Directory.Exists(destino))
                        Directory.Delete(destino, true);

                    CopyDirectory(origem, destino);

                    ShowToast(
                    "RDAoficial",
                    "Backup Realizado com sucesso!"
                   );
                }
                else
                {
                    ShowToast(
                    "RDAoficial",
                    "Erro: Pasta não encontrada!"
                   );
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        // ==========================================
        // RESTAURAR AUTOMÁTICO
        // ==========================================

        private void BtnRestore_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string origem =
                    Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                        "stplug-in"
                    );

                string destino =
                    @"C:\Program Files (x86)\Steam\config\stplug-in";

                if (Directory.Exists(destino))
                    Directory.Delete(destino, true);

                Directory.Move(origem, destino);

                ShowToast(
                    "RDAoficial",
                    "Backup Restaurado com sucesso!"
                   );
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        // ==========================================
        // BACKUP CONQUISTAS AUTOMÁTICAMENTE
        // ==========================================

        private void BtnBackupconq_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string origem =
                    @"C:\Program Files (x86)\Steam\appcache\stats";

                string destino =
                    Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                        "stats"
                    );

                if (Directory.Exists(origem))
                {
                    if (Directory.Exists(destino))
                        Directory.Delete(destino, true);

                    CopyDirectory(origem, destino);

                    ShowToast(
                    "RDAoficial",
                    "Backup Realizado com sucesso!"
                   );
                }
                else
                {
                    ShowToast(
                    "RDAoficial",
                    "Erro: Pasta não encontrada!"
                   );
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        // ==========================================
        // RESTAURAR CONQUISTAS AUTOMÁTICAMENTE
        // ==========================================

        private void BtnRestoreconq_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string origem =
                    Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                        "stats"
                    );

                string destino =
                    @"C:\Program Files (x86)\Steam\appcache\stats";

                if (Directory.Exists(destino))
                    Directory.Delete(destino, true);

                Directory.Move(origem, destino);

                ShowToast(
                    "RDAoficial",
                    "Backup Restaurado com sucesso!"
                   );
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }


        // ==========================================
        // BAIXAR INSTALADOR APP LUA
        // ==========================================

        private void BtnSalvarLuaInstaller_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://lua.tools/app",
                UseShellExecute = true
            });
        }



        // ==========================================
        // SAIR
        // ==========================================

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnMillennium_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/SteamClientHomebrew/Millennium/releases",
                UseShellExecute = true
            });
        }

        private void BtnSelecionarLua_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    dialog.Description = "Selecione a pasta (extraida) do plugin";

                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string pastaSelecionada = dialog.SelectedPath;

                        string pastaDestino =
                            @"C:\Program Files (x86)\Steam\millennium\plugins";

                        // Nome da pasta escolhida
                        string nomePasta =
                            System.IO.Path.GetFileName(pastaSelecionada);

                        // Destino final
                        string destinoFinal =
                            System.IO.Path.Combine(pastaDestino, nomePasta);

                        // Cria diretório base se não existir
                        if (!Directory.Exists(pastaDestino))
                        {
                            Directory.CreateDirectory(pastaDestino);
                        }

                        // Remove antiga se já existir
                        if (Directory.Exists(destinoFinal))
                        {
                            Directory.Delete(destinoFinal, true);
                        }

                        // Copia pasta inteira
                        CopyDirectory(pastaSelecionada, destinoFinal);

                        ShowToast(
                            "RDAoficial",
                            "Instalação/atualização do plugin concluida!!"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                ShowToast(
                    "Erro",
                    ex.Message
                );
            }
        }

        private void CopyDirectory(string sourceDir, string destinationDir)
        {
            Directory.CreateDirectory(destinationDir);

            foreach (string file in Directory.GetFiles(sourceDir))
            {
                string destFile =
                    Path.Combine(destinationDir, Path.GetFileName(file));

                File.Copy(file, destFile, true);
            }

            foreach (string folder in Directory.GetDirectories(sourceDir))
            {
                string destFolder =
                    Path.Combine(destinationDir, Path.GetFileName(folder));

                CopyDirectory(folder, destFolder);
            }

        }


        private void MostrarSobre()
        {
            Window sobre = new Window
            {
                Title = "Sobre",
                Width = 500,
                Height = 350,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                Background = Brushes.Transparent,
                Opacity = 0
            };

            Border border = new Border
            {
                CornerRadius = new CornerRadius(18),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#151515")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2A2A2A")),
                BorderThickness = new Thickness(1),
                Padding = new Thickness(25),
                RenderTransform = new TranslateTransform(0, 30)
            };

            StackPanel panel = new StackPanel();

            TextBlock titulo = new TextBlock
            {
                Text = "RDAoficial",
                FontSize = 26,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Gold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 15)
            };

            TextBlock texto = new TextBlock
            {
                Text =
        @"Versão: 2.1.4

Ferramenta desenvolvida para auxiliar no gerenciamento do Steam Tools e Lua Tools.

Desenvolvido por: RDAoficial (Youtuber)

© 2026 Todos os direitos reservados.",
                Foreground = Brushes.White,
                FontSize = 14,
                TextWrapping = TextWrapping.Wrap
            };

            Button fechar = new Button
            {
                Content = "Fechar",
                Width = 120,
                Height = 36,
                FontWeight = FontWeights.SemiBold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 0),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0000")),
                Foreground = Brushes.Black,
                BorderThickness = new Thickness(0),
                Cursor = System.Windows.Input.Cursors.Hand
            };

            fechar.Click += (s, e) => sobre.Close();

            panel.Children.Add(titulo);
            panel.Children.Add(texto);
            panel.Children.Add(fechar);
            border.Child = panel;
            sobre.Content = border;

            sobre.Loaded += (s, e) =>
            {
                this.Effect = new BlurEffect
                {
                    Radius = 6
                };

                var fade = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(180));
                sobre.BeginAnimation(Window.OpacityProperty, fade);

                var slide = new DoubleAnimation(30, 0, TimeSpan.FromMilliseconds(250));
                border.RenderTransform.BeginAnimation(TranslateTransform.YProperty, slide);
            };

            sobre.Closed += (s, e) =>
            {
                this.Effect = null;
            };

            sobre.ShowDialog();
        }
    
    
    }
}