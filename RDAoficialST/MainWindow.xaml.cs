using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Forms = System.Windows.Forms;
using Microsoft.Win32;
using System.Reflection;

namespace RDAoficialST
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowModernMessage(string titulo, string mensagem)
        {
            Window msg = new Window();

            msg.Title = titulo;
            msg.Width = 420;
            msg.Height = 220;
            msg.ResizeMode = ResizeMode.NoResize;
            msg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            msg.Background = new System.Windows.Media.BrushConverter().ConvertFromString("#151515") as System.Windows.Media.Brush;
            msg.WindowStyle = WindowStyle.None;
            msg.AllowsTransparency = true;

            Border border = new Border();
            border.CornerRadius = new CornerRadius(18);
            border.Background = new System.Windows.Media.BrushConverter().ConvertFromString("#151515") as System.Windows.Media.Brush;
            border.BorderBrush = new System.Windows.Media.BrushConverter().ConvertFromString("#2A2A2A") as System.Windows.Media.Brush;
            border.BorderThickness = new Thickness(1);
            border.Padding = new Thickness(25);

            StackPanel panel = new StackPanel();

            TextBlock titleBlock = new TextBlock();
            titleBlock.Text = titulo;
            titleBlock.FontSize = 24;
            titleBlock.FontWeight = FontWeights.Bold;
            titleBlock.Foreground = System.Windows.Media.Brushes.White;
            titleBlock.Margin = new Thickness(0, 0, 0, 15);

            TextBlock messageBlock = new TextBlock();
            messageBlock.Text = mensagem;
            messageBlock.FontSize = 14;
            messageBlock.Foreground = System.Windows.Media.Brushes.White;
            messageBlock.TextWrapping = TextWrapping.Wrap;
            messageBlock.Margin = new Thickness(0, 0, 0, 20);

            Button okButton = new Button();
            okButton.Content = "OK";
            okButton.Width = 100;
            okButton.Height = 38;
            okButton.HorizontalAlignment = HorizontalAlignment.Center;
            okButton.Background = new System.Windows.Media.BrushConverter().ConvertFromString("#EF00FC") as System.Windows.Media.Brush;
            okButton.Foreground = System.Windows.Media.Brushes.White;
            okButton.BorderThickness = new Thickness(0);
            okButton.Cursor = System.Windows.Input.Cursors.Hand;

            okButton.Click += (s, e) =>
            {
                msg.Close();
            };

            panel.Children.Add(titleBlock);
            panel.Children.Add(messageBlock);
            panel.Children.Add(okButton);

            border.Child = panel;

            msg.Content = border;

            msg.ShowDialog();
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
        // SITE INCORPORADO
        // ==========================================

        private void BtnAbrirSite_Click(object sender, RoutedEventArgs e)
        {
            SiteWindow janela = new SiteWindow();

            janela.ShowDialog();
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
        // INSTALAR
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
                    UseShellExecute = true
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

            ShowModernMessage(
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

                    ShowModernMessage(
                    "RDAoficial",
                    "Backup Realizado com sucesso!"
                   );
                }
                else
                {
                    ShowModernMessage(
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

                ShowModernMessage(
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

                                ShowModernMessage(
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
                                ShowModernMessage(
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

        private void BtnSalvarExe_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();

                saveDialog.Filter = "Executável (*.exe)|*.exe";
                saveDialog.FileName = "MillenniumInstaller-Windows.exe";

                if (saveDialog.ShowDialog() == true)
                {
                    string resourceName =
                        "RDAoficialST.Assets.MillenniumInstaller-Windows.exe";

                    using (Stream stream =
                        Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream(resourceName))
                    {
                        if (stream == null)
                        {
                            ShowModernMessage(
                                "Erro",
                                "O arquivo incorporado não foi encontrado."
                            );

                            return;
                        }

                        using (FileStream file =
                            new FileStream(saveDialog.FileName, FileMode.Create))
                        {
                            stream.CopyTo(file);
                        }
                    }

                    ShowModernMessage(
                        "RDAoficial",
                        "Aplicativo salvo com sucesso!"
                    );
                }
            }
            catch (Exception ex)
            {
                ShowModernMessage(
                    "Erro",
                    ex.Message
                );
            }
        }

        private void BtnSelecionarLua_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    dialog.Description = "Selecione a pasta do plugin";

                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string pastaSelecionada = dialog.SelectedPath;

                        string pastaDestino =
                            @"C:\Program Files (x86)\Steam\plugins";

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

                        ShowModernMessage(
                            "RDAoficial",
                            "Instalação/atualização do plugin concluida!!"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                ShowModernMessage(
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
    }
}