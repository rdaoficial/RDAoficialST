using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Forms = System.Windows.Forms;
using System.Windows.Media.Effects;
using Path = System.IO.Path;
using System.Windows.Markup;

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
        // GITHUB PLUGIN LUA
        // ==========================================
        private void BtnbaixarLua_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/madoiscool/ltsteamplugin/releases",
                UseShellExecute = true
            });
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
                FileName = "https://www.youtube.com/watch?v=8BYgTjpeKlc&t=461s",
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



        // ==========================================
        // DISCORD
        // ==========================================

        private void BtnDiscord_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://discord.gg/uAJPSupsbx",
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
        // INSTALAR (TUDO) (MAIS RECENTE)
        // ==========================================

        private void BtnInstall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cmd =
                    "Invoke-RestMethod \\\"https://luatools.vercel.app/install-plugin.ps1\\\" | Invoke-Expression";

                Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $" -Command {cmd}",
                    UseShellExecute = true,
                    Verb = "runas"
                });
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    ex.Message,
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }



        // ==========================================
        // INSTALAR (TUDO) (LEGACY)
        // ==========================================

        private void BtnInstalllegacy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cmd =
                    "Invoke-RestMethod \\\"https://luatoolsplugin.vercel.app/install-st-ml-lt-cl.ps1\\\" | Invoke-Expression";

                Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $" -Command {cmd}",
                    UseShellExecute = true,
                    Verb = "runas"
                });
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    ex.Message,
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        // ==========================================
        // REMOVER
        // ==========================================

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            string[] caminhos =
            {
                @"C:\Program Files (x86)\Steam\ext",
                @"C:\Program Files (x86)\Steam\plugins",
                @"C:\Program Files (x86)\Steam\dwmapi.dll",
                @"C:\Program Files (x86)\Steam\millennium.dll",
                @"C:\Program Files (x86)\Steam\millennium.hhx64.dll",
                @"C:\Program Files (x86)\Steam\millennium-legacy.version.dll",
                @"C:\Program Files (x86)\Steam\python311.dll",
                @"C:\Program Files (x86)\Steam\wsock32.dll",
                @"C:\Program Files (x86)\Steam\millennium",
                @"C:\Program Files (x86)\Steam\millennium-migration-temp",
                @"C:\Program Files (x86)\Steam\xinput1_4.dll"
            };

            foreach (string caminho in caminhos)
            {
                try
                {
                    if (Directory.Exists(caminho))
                        Directory.Delete(caminho, true);

                    if (File.Exists(caminho))
                        File.Delete(caminho);
                }
                catch
                {
                }
            }

            ShowToast(
                   "RDAoficial",
                   "Exclusão concluida!"
                  );
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

                    Directory.Move(origem, destino);

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
        // BACKUP MANUAL
        // ==========================================

        private void BtnBackupManual_Click(object sender, RoutedEventArgs e)
        {
            using (Forms.FolderBrowserDialog origemDialog = new Forms.FolderBrowserDialog())
            {
                origemDialog.Description = "Selecione a pasta (stplug-in) no diretorio da sua steam";

                if (origemDialog.ShowDialog() == Forms.DialogResult.OK)
                {
                    using (Forms.FolderBrowserDialog destinoDialog = new Forms.FolderBrowserDialog())
                    {
                        destinoDialog.Description = "Selecione onde salvar o backup";

                        if (destinoDialog.ShowDialog() == Forms.DialogResult.OK)
                        {
                            try
                            {
                                string origem = origemDialog.SelectedPath;

                                string destino =
                                    Path.Combine(
                                        destinoDialog.SelectedPath,
                                        "stplug-in"
                                    );

                                if (Directory.Exists(destino))
                                    Directory.Delete(destino, true);

                                Directory.Move(origem, destino);

                                ShowToast(
                                    "RDAoficial",
                                    "Backup Feito!"
                                );
                            }
                            catch (Exception ex)
                            {
                                System.Windows.MessageBox.Show(ex.Message);
                            }
                        }
                    }
                }
            }
        }

        // ==========================================
        // RESTAURAR MANUAL
        // ==========================================

        private void BtnRestoreManual_Click(object sender, RoutedEventArgs e)
        {
            using (Forms.FolderBrowserDialog origemDialog = new Forms.FolderBrowserDialog())
            {
                origemDialog.Description = "Selecione a pasta de backup stplug-in";

                if (origemDialog.ShowDialog() == Forms.DialogResult.OK)
                {
                    using (Forms.FolderBrowserDialog destinoDialog = new Forms.FolderBrowserDialog())
                    {
                        destinoDialog.Description = "Selecione a pasta config da Steam";

                        if (destinoDialog.ShowDialog() == Forms.DialogResult.OK)
                        {
                            try
                            {
                                string origem = origemDialog.SelectedPath;

                                string destino =
                                    Path.Combine(
                                        destinoDialog.SelectedPath,
                                        "stplug-in"
                                    );

                                if (Directory.Exists(destino))
                                    Directory.Delete(destino, true);

                                Directory.Move(origem, destino);
                                ShowToast(
                                    "RDAoficial",
                                    "Backup Restaurado!"
                                );
                            }
                            catch (Exception ex)
                            {
                                System.Windows.MessageBox.Show(ex.Message);
                            }
                        }
                    }
                }
            }
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
        @"Versão: 2.1.2

Ferramenta desenvolvida para auxiliar na instalação
e gerenciamento do Steam Tools, Millennium e Lua Tools.

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
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD700")),
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