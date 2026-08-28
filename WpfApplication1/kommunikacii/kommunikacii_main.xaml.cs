using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using Path = System.IO.Path;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Net.Http;

namespace TreeCadN.kommunikacii
{
    /// <summary>
    /// Логика взаимодействия для kommunikacii.xaml
    /// </summary>
    public partial class kommunikacii_main : Window
    {
        private static readonly HttpClient httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromMinutes(5)
        };

        public string pathBD = "";
        public string selectedModel = "";
        BD_Connect BD = new BD_Connect();
        int lb1_selitem, lb4_selitem;
        private CancellationTokenSource serverModelsLoadCancellation;
        private CancellationTokenSource localModelsLoadCancellation;
        private CancellationTokenSource actionCancellation;
        private bool isBusy;
        private bool initialLoadCompleted;
        List<Models3d> server_obj = new List<Models3d>();
        string dir3ds, dir3ds_user;
        string admin;
        public kommunikacii_main(string pathBD)
        {
            InitializeComponent();
            log.Add("старт диалог моделей");
            dir3ds = Environment.CurrentDirectory + @"\3dsObject\Web";
            dir3ds_user = Environment.CurrentDirectory + @"\3dsObject\User";

            Directory.CreateDirectory(dir3ds);
            Directory.CreateDirectory(dir3ds_user);

            this.pathBD = pathBD;
            BD.path = pathBD; //укажем файл бд



            INIManager client_man = new INIManager(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\ecadpro.ini");
            admin = client_man.GetPrivateString("Infogen", "3dsadmin");//версия клиента
            if (admin == "1")
            {
                label1.Visibility = Visibility.Visible;
                allnaserver.Visibility = Visibility.Visible;

            }
            else
            {
                lb4.ContextMenu = null;
                label1.Visibility = Visibility.Collapsed;
                allnaserver.Visibility = Visibility.Collapsed;
            }


            move_to_new_41();
            loadpage();
            lb2napolnene();
            RestoreLocalCategorySelection();
            UpdateActionStates();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (initialLoadCompleted)
            {
                return;
            }

            initialLoadCompleted = true;
            await LoadServerCategoriesAsync();
        }

        private void RestoreLocalCategorySelection()
        {
            int index = kommunikacii_Set.Default.lb2sel;
            if (index >= 0 && index < lb2.Items.Count)
            {
                lb2.SelectedIndex = index;
            }
        }

        private void SetBusy(bool value, string message)
        {
            isBusy = value;
            busyProgress.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            statusText.Text = string.IsNullOrWhiteSpace(message) ? "Готово" : message;
            UpdateActionStates();
        }

        private void UpdateActionStates()
        {
            if (okButton == null)
            {
                return;
            }

            bool hasServerModel = lb4 != null && lb4.SelectedItem is Models3d;
            treespis localCategory = lb2 == null ? null : lb2.SelectedItem as treespis;
            bool hasLocalCategory = localCategory != null;
            bool hasSpecificLocalCategory = hasLocalCategory && !localCategory.is_all;
            bool hasLocalModel = lb1 != null && lb1.SelectedItem is Models3d;
            bool serverTabSelected = item1 != null && item1.IsSelected;

            okButton.IsEnabled = !isBusy && (serverTabSelected ? hasServerModel : hasLocalModel);
            addLocalModelButton.IsEnabled = !isBusy && hasSpecificLocalCategory;
            editLocalModelButton.IsEnabled = !isBusy && hasSpecificLocalCategory && hasLocalModel;
            allnaserver.IsEnabled = !isBusy && hasSpecificLocalCategory && lb1.Items.Count > 0;
            deleteLocalCategoryMenuItem.IsEnabled = !isBusy && hasSpecificLocalCategory;
            peremestitb_serv.IsEnabled = !isBusy && hasServerModel;
            delete_server_object.IsEnabled = !isBusy && hasServerModel;
            sendLocalModelMenuItem.IsEnabled = !isBusy && hasSpecificLocalCategory && hasLocalModel;
            editLocalModelMenuItem.IsEnabled = !isBusy && hasSpecificLocalCategory && hasLocalModel;
            deleteLocalModelMenuItem.IsEnabled = !isBusy && hasLocalModel;
        }

        private void ListBox_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            DependencyObject source = e.OriginalSource as DependencyObject;
            if (listBox == null || source == null)
            {
                return;
            }

            ListBoxItem item = ItemsControl.ContainerFromElement(listBox, source) as ListBoxItem;
            if (item != null)
            {
                item.IsSelected = true;
                item.Focus();
            }
            else
            {
                listBox.SelectedItem = null;
            }
        }

        private static bool ModelMatchesSearch(object value, string searchText)
        {
            Models3d model = value as Models3d;
            if (model == null)
            {
                return true;
            }

            return (model.name ?? string.Empty).IndexOf(searchText,
                StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        private void serverSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyModelFilter(lb4, serverSearchBox.Text);
        }

        private void localSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyModelFilter(lb1, localSearchBox.Text);
        }

        private static void ApplyModelFilter(ListBox listBox, string searchText)
        {
            if (listBox == null || listBox.ItemsSource == null)
            {
                return;
            }

            ICollectionView view = CollectionViewSource.GetDefaultView(listBox.ItemsSource);
            string normalizedSearchText = (searchText ?? string.Empty).Trim();
            if (normalizedSearchText.Length < 2)
            {
                if (view.Filter == null)
                {
                    return;
                }

                view.Filter = null;
                view.Refresh();
                return;
            }

            view.Filter = delegate(object value) { return ModelMatchesSearch(value, normalizedSearchText); };
            view.Refresh();
        }


        void lb3napolneneeeee()
        {

        }
        void lb2napolnene()
        {
            List<treespis> spisvategor = new List<treespis>
            {
                new treespis { name = "Все", is_all = true }
            };

            foreach (string elem in Directory.GetDirectories(dir3ds_user, "*", SearchOption.TopDirectoryOnly)
                .OrderBy(delegate(string path) { return Path.GetFileName(path); }, StringComparer.CurrentCultureIgnoreCase))
            {
                spisvategor.Add(new treespis { name = Path.GetFileName(elem) });
            }

            lb2.ItemsSource = spisvategor;
        }



        public async void lb1_napolnenie()//наполнение листбокса
        {
            treespis category = lb2.SelectedItem as treespis;
            if (category == null)
            {
                lb1.ItemsSource = null;
                selectedModel = string.Empty;
                UpdateActionStates();
                return;
            }

            if (localModelsLoadCancellation != null)
            {
                localModelsLoadCancellation.Cancel();
            }

            CancellationTokenSource cancellation = new CancellationTokenSource();
            localModelsLoadCancellation = cancellation;
            string completionMessage = "Готово";
            SetBusy(true, "Загрузка пользовательских объектов…");

            try
            {
                List<Models3d> models = await Task.Run(
                    delegate { return LoadLocalModels(category, cancellation.Token); },
                    cancellation.Token);

                cancellation.Token.ThrowIfCancellationRequested();
                treespis selectedCategory = lb2.SelectedItem as treespis;
                if (selectedCategory == null || !string.Equals(selectedCategory.name, category.name, StringComparison.Ordinal))
                {
                    return;
                }

                lb1.ItemsSource = models;
                ApplyModelFilter(lb1, localSearchBox.Text);

                if (lb1_selitem >= 0 && lb1_selitem < lb1.Items.Count)
                {
                    object selectedItem = lb1.Items[lb1_selitem];
                    lb1.SelectedItem = selectedItem;
                    lb1.ScrollIntoView(selectedItem);
                }
                else
                {
                    lb1.SelectedItem = null;
                }

                completionMessage = models.Count == 0
                    ? "В категории нет 3DS-объектов"
                    : "Загружено объектов: " + models.Count;
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                log.Add("Ошибка загрузки пользовательских объектов: " + ex.Message);
                completionMessage = "Не удалось загрузить пользовательские объекты";
                MessageBox.Show(completionMessage + ".\n" + ex.Message,
                    "3D объекты", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (ReferenceEquals(localModelsLoadCancellation, cancellation))
                {
                    localModelsLoadCancellation = null;
                    cancellation.Dispose();
                    SetBusy(false, completionMessage);
                }
            }
        }

        private List<Models3d> LoadLocalModels(treespis category, CancellationToken cancellationToken)
        {
            List<Models3d> models = new List<Models3d>();
            IEnumerable<string> categoryPaths = category.is_all
                ? Directory.GetDirectories(dir3ds_user, "*", SearchOption.TopDirectoryOnly)
                : new[] { Path.Combine(dir3ds_user, category.name) };

            foreach (string categoryPath in categoryPaths)
            {
                foreach (string modelDirectory in Directory.GetDirectories(categoryPath, "*", SearchOption.TopDirectoryOnly))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    try
                    {
                        string name = Path.GetFileName(modelDirectory);
                        string[] modelFiles = Directory.GetFiles(modelDirectory, "*.3ds", SearchOption.TopDirectoryOnly);
                        if (modelFiles.Length == 0)
                        {
                            continue;
                        }

                        string modelPath = modelFiles[0];
                        string technicalDirectory = Path.Combine(modelDirectory, "teh");
                        string previewPath = Path.Combine(technicalDirectory, "prev.jpg");
                        string anglePreviewPath = Path.Combine(technicalDirectory, "ugo.jpg");
                        string configPath = Path.Combine(technicalDirectory, "conf.json");
                        Models3dJSON dimensions = null;

                        if (File.Exists(configPath))
                        {
                            dimensions = JsonConvert.DeserializeObject<Models3dJSON>(
                                File.ReadAllText(configPath, Encoding.Default));
                        }

                        if (dimensions == null)
                        {
                            try
                            {
                                cancellationToken.ThrowIfCancellationRequested();
                                Model3D model = (new HelixToolkit.Wpf.ModelImporter()).Load(modelPath);
                                dimensions = new Models3dJSON
                                {
                                    x = model.Bounds.SizeX.ToString("0"),
                                    z = model.Bounds.SizeY.ToString("0"),
                                    y = model.Bounds.SizeZ.ToString("0")
                                };
                            }
                            catch
                            {
                                dimensions = new Models3dJSON { x = "100", y = "100", z = "100" };
                            }

                            Directory.CreateDirectory(technicalDirectory);
                            File.WriteAllText(configPath, JsonConvert.SerializeObject(dimensions), Encoding.Default);
                        }

                        models.Add(new Models3d
                        {
                            name = name,
                            path = modelPath,
                            jpg_path = previewPath,
                            jpg_ugo = anglePreviewPath,
                            pathcategory = modelDirectory,
                            x = dimensions.x,
                            y = dimensions.y,
                            z = dimensions.z,
                            bitmap_prev = LoadThumbnail(previewPath)
                        });
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        log.Add("Ошибка чтения объекта " + modelDirectory + ": " + ex.Message);
                    }
                }
            }

            return models;
        }

        private static BitmapImage LoadThumbnail(string filePath)
        {
            Uri source = File.Exists(filePath)
                ? new Uri(filePath, UriKind.Absolute)
                : new Uri("pack://application:,,,/TreeCadN;component/Foto/nofoto.jpg", UriKind.Absolute);

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.DecodePixelWidth = 150;
            image.UriSource = source;
            image.EndInit();
            if (image.CanFreeze)
            {
                image.Freeze();
            }

            return image;
        }

        List<treespis> server;

        private async Task LoadServerCategoriesAsync()
        {
            SetBusy(true, "Загрузка категорий с сервера…");
            try
            {
                string json = await Task.Run(delegate { return loadsinch("load_categor"); });
                if (string.IsNullOrWhiteSpace(json))
                {
                    throw new InvalidOperationException("Сервер не вернул список категорий.");
                }

                List<treespis> loadedCategories = JsonConvert.DeserializeObject<List<treespis>>(json);
                if (loadedCategories == null)
                {
                    throw new InvalidOperationException("Не удалось прочитать список категорий.");
                }

                server = loadedCategories.Where(delegate(treespis item) { return item != null; }).ToList();
                peremestitb_serv.Items.Clear();
                List<treespis> displayedCategories = new List<treespis>
                {
                    new treespis { name = "Все", is_all = true }
                };
                displayedCategories.AddRange(server.OrderBy(
                    delegate(treespis item) { return item.name ?? string.Empty; },
                    StringComparer.CurrentCultureIgnoreCase));
                lb3.ItemsSource = displayedCategories;

                foreach (treespis category in server)
                {
                    System.Windows.Controls.MenuItem menuItem = new System.Windows.Controls.MenuItem
                    {
                        Tag = category,
                        Header = category.name ?? "Без названия"
                    };
                    menuItem.Click += delegate { peremestitb_serv_event(category); };
                    peremestitb_serv.Items.Add(menuItem);
                }

                statusText.Text = server.Count == 0
                    ? "Категории на сервере не найдены"
                    : "Загружено категорий: " + server.Count;
            }
            catch (Exception ex)
            {
                log.Add("Ошибка загрузки категорий: " + ex.Message);
                MessageBox.Show("Не удалось загрузить категории с сервера.\n" + ex.Message,
                    "3D объекты", MessageBoxButton.OK, MessageBoxImage.Error);
                statusText.Text = "Ошибка загрузки категорий";
            }
            finally
            {
                SetBusy(false, statusText.Text);
            }
        }

        private async void lb3napolnene()
        {
            await LoadServerCategoriesAsync();
        }

        private async void lb4_napolnenie()//наполнение листбокса
        {
            treespis category = lb3.SelectedItem as treespis;
            if (category == null)
            {
                lb4.ItemsSource = null;
                label1.Content = "Выберите категорию";
                selectedModel = string.Empty;
                UpdateActionStates();
                return;
            }

            if (serverModelsLoadCancellation != null)
            {
                serverModelsLoadCancellation.Cancel();
            }

            CancellationTokenSource cancellation = new CancellationTokenSource();
            serverModelsLoadCancellation = cancellation;
            string completionMessage = "Готово";
            SetBusy(true, "Загрузка объектов с сервера…");

            try
            {
                List<Models3d> models = await Task.Run(
                    delegate { return LoadServerModels(category, cancellation.Token); },
                    cancellation.Token);

                cancellation.Token.ThrowIfCancellationRequested();
                foreach (Models3d model in models)
                {
                    cancellation.Token.ThrowIfCancellationRequested();
                    if (model != null && Uri.IsWellFormedUriString(model.jpg_path, UriKind.Absolute))
                    {
                        BitmapImage preview = new BitmapImage();
                        preview.BeginInit();
                        preview.DecodePixelWidth = 150;
                        preview.UriSource = new Uri(model.jpg_path, UriKind.Absolute);
                        preview.EndInit();
                        model.jpgsource = preview;
                    }
                    else if (model != null)
                    {
                        model.jpgsource = LoadThumbnail(null);
                    }
                }

                treespis currentCategory = lb3.SelectedItem as treespis;
                if (currentCategory == null || currentCategory.is_all != category.is_all
                    || (!category.is_all && !string.Equals(currentCategory.id_server,
                        category.id_server, StringComparison.Ordinal)))
                {
                    return;
                }

                lb4.ItemsSource = models;
                ApplyModelFilter(lb4, serverSearchBox.Text);
                if (lb4_selitem >= 0 && lb4_selitem < lb4.Items.Count)
                {
                    object selectedItem = lb4.Items[lb4_selitem];
                    lb4.SelectedItem = selectedItem;
                    lb4.ScrollIntoView(selectedItem);
                }

                completionMessage = models.Count == 0
                    ? "В категории нет объектов"
                    : "Загружено объектов: " + models.Count;
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                log.Add("Ошибка загрузки серверных объектов: " + ex.Message);
                completionMessage = "Не удалось загрузить объекты с сервера";
                MessageBox.Show(completionMessage + ".\n" + ex.Message,
                    "3D объекты", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (ReferenceEquals(serverModelsLoadCancellation, cancellation))
                {
                    serverModelsLoadCancellation = null;
                    cancellation.Dispose();
                    SetBusy(false, completionMessage);
                }
            }
        }

        private List<Models3d> LoadServerModels(treespis category, CancellationToken cancellationToken)
        {
            if (!category.is_all)
            {
                string categoryId = Uri.EscapeDataString(category.id_server ?? string.Empty);
                string json = loadsinch("load_object", "category=" + categoryId);
                if (string.IsNullOrWhiteSpace(json))
                {
                    throw new InvalidOperationException("Сервер не вернул список объектов.");
                }

                return JsonConvert.DeserializeObject<List<Models3d>>(json) ?? new List<Models3d>();
            }

            List<Models3d> result = new List<Models3d>();
            HashSet<string> addedModels = new HashSet<string>(StringComparer.Ordinal);
            IEnumerable<string> categoryIds = server
                .Where(delegate(treespis item) { return item != null && !string.IsNullOrWhiteSpace(item.id_server); })
                .Select(delegate(treespis item) { return item.id_server; })
                .Distinct(StringComparer.Ordinal);

            foreach (string id in categoryIds)
            {
                cancellationToken.ThrowIfCancellationRequested();
                string json = loadsinch("load_object", "category=" + Uri.EscapeDataString(id));
                if (string.IsNullOrWhiteSpace(json))
                {
                    continue;
                }

                List<Models3d> categoryModels;
                try
                {
                    categoryModels = JsonConvert.DeserializeObject<List<Models3d>>(json) ?? new List<Models3d>();
                }
                catch (JsonException ex)
                {
                    log.Add("Не удалось прочитать объекты категории " + id + ": " + ex.Message);
                    continue;
                }

                foreach (Models3d model in categoryModels)
                {
                    if (model == null)
                    {
                        continue;
                    }

                    string key = !string.IsNullOrWhiteSpace(model.id_server)
                        ? "server:" + model.id_server
                        : !string.IsNullOrWhiteSpace(model.id)
                            ? "id:" + model.id
                            : "path:" + (model.path ?? string.Empty) + "|" + (model.name ?? string.Empty);
                    if (addedModels.Add(key))
                    {
                        result.Add(model);
                    }
                }
            }

            return result;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            lb1_redak();
        }

        void lb1_redak()
        {
            treespis selectedCategory = lb2.SelectedItem as treespis;
            if (selectedCategory != null && selectedCategory.is_all)
            {
                MessageBox.Show("Для редактирования выберите конкретную категорию.");
                return;
            }

            if (lb2.SelectedIndex > -1)
            {
                if (lb1.SelectedIndex > -1)
                {
                    var file = lb1.SelectedItem as Models3d;

                    kommunikacii_dial_imp dial = new kommunikacii_dial_imp(this, pathBD, file, (lb2.SelectedItem as treespis));
                    dial.ShowDialog();

                    //  lb1_napolnenie();
                }
                else
                {
                    MessageBox.Show("Сначала выберите объект для редактирования");
                }
            }
            else
            {
                MessageBox.Show("Сначала выберите категорию");
            }

        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {

            dob_new3ds();


   
        }
        void dob_new3ds()
        {
            treespis selectedCategory = lb2.SelectedItem as treespis;
            if (selectedCategory != null && selectedCategory.is_all)
            {
                MessageBox.Show("Для добавления объекта выберите конкретную категорию.");
                return;
            }

            if (lb2.SelectedIndex > -1)
            {
                //  var model = (lb2.SelectedItem as treespis).id;
                kommunikacii_dial_imp dial = new kommunikacii_dial_imp(this, pathBD, null, (lb2.SelectedItem as treespis));
                dial.ShowDialog();
                //     MessageBox.Show("Модель успешно создана");
                //lb1_napolnenie();
            }
            else
            {
                MessageBox.Show("Сначала выберите категорию");
            }
            //  lb1_napolnenie();



        }

        private void lb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lb1.SelectedItem != null)
            {
                var file = lb1.SelectedValue as Models3d;
                SetSelectedLocalModel(file);
                lb1_selitem = lb1.SelectedIndex;
            }
            else if (item2.IsSelected)
            {
                selectedModel = string.Empty;
            }

            UpdateActionStates();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (isBusy)
            {
                return;
            }

            Models3d selected = item1.IsSelected
                ? lb4.SelectedItem as Models3d
                : lb1.SelectedItem as Models3d;
            if (selected == null)
            {
                MessageBox.Show("Сначала выберите 3D-объект.", "3D объекты");
                return;
            }

            actionCancellation = new CancellationTokenSource();
            try
            {
                SetBusy(true, item1.IsSelected ? "Скачивание выбранного объекта…" : "Подготовка объекта…");
                if (item1.IsSelected)
                {
                    await PrepareServerModelAsync(selected, actionCancellation.Token);
                }
                else
                {
                    SetSelectedLocalModel(selected);
                }

                Close();
            }
            catch (OperationCanceledException)
            {
                SetBusy(false, "Операция отменена");
            }
            catch (Exception ex)
            {
                log.Add("Ошибка подготовки выбранного объекта: " + ex.Message);
                SetBusy(false, "Не удалось подготовить объект");
                MessageBox.Show("Не удалось подготовить выбранный объект.\n" + ex.Message,
                    "3D объекты", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (actionCancellation != null)
                {
                    actionCancellation.Dispose();
                    actionCancellation = null;
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (actionCancellation != null)
            {
                actionCancellation.Cancel();
            }
            Close();
        }




        private void SetSelectedLocalModel(Models3d model)
        {
            selectedModel = model.path + "^" + model.name + "^" + model.jpg_ugo + "^"
                + model.x + "^" + model.y + "^" + model.z;
        }

        private async Task PrepareServerModelAsync(Models3d model, CancellationToken cancellationToken)
        {
            if (model == null)
            {
                throw new InvalidOperationException("Объект не выбран.");
            }

            string targetDirectory = Path.Combine(dir3ds, model.category ?? string.Empty, model.name ?? string.Empty);
            Directory.CreateDirectory(targetDirectory);
            string[] modelFiles = Directory.GetFiles(targetDirectory, "*.3ds", SearchOption.TopDirectoryOnly);
            string temporaryArchive = null;

            try
            {
                if (modelFiles.Length == 0)
                {
                    string metadataUrl = "https://ecad.giulianovars.ru/php/3dsobject/3ds.php?command=getZip&id="
                        + Uri.EscapeDataString(model.id ?? string.Empty);
                    using (HttpResponseMessage metadataResponse = await httpClient.GetAsync(metadataUrl, cancellationToken))
                    {
                        metadataResponse.EnsureSuccessStatusCode();
                        string archiveUrl = (await metadataResponse.Content.ReadAsStringAsync()).Trim();
                        Uri parsedArchiveUrl;
                        if (!Uri.TryCreate(archiveUrl, UriKind.Absolute, out parsedArchiveUrl))
                        {
                            throw new InvalidDataException("Сервер вернул некорректный адрес архива.");
                        }

                        using (HttpResponseMessage archiveResponse = await httpClient.GetAsync(parsedArchiveUrl, cancellationToken))
                        {
                            archiveResponse.EnsureSuccessStatusCode();
                            byte[] archiveBytes = await archiveResponse.Content.ReadAsByteArrayAsync();
                            cancellationToken.ThrowIfCancellationRequested();

                            temporaryArchive = Path.Combine(Path.GetTempPath(),
                                "3ds_load_TreeCadN_" + Guid.NewGuid().ToString("N") + ".zip");
                            await Task.Run(delegate
                            {
                                cancellationToken.ThrowIfCancellationRequested();
                                File.WriteAllBytes(temporaryArchive, archiveBytes);
                                ZipFile.ExtractToDirectory(temporaryArchive, targetDirectory);
                            }, cancellationToken);
                        }
                    }

                    modelFiles = Directory.GetFiles(targetDirectory, "*.3ds", SearchOption.TopDirectoryOnly);
                }

                if (modelFiles.Length == 0)
                {
                    throw new FileNotFoundException("В архиве не найден 3DS-файл.");
                }

                string anglePreview = model.loc_ugojpg;
                string localAnglePreview = Directory.GetFiles(targetDirectory, "ugo.jpg", SearchOption.AllDirectories)
                    .FirstOrDefault();
                if (!string.IsNullOrEmpty(localAnglePreview))
                {
                    anglePreview = localAnglePreview;
                }

                selectedModel = modelFiles[0] + "^" + model.name + "^" + anglePreview + "^"
                    + model.x + "^" + model.y + "^" + model.z;
            }
            finally
            {
                if (!string.IsNullOrEmpty(temporaryArchive) && File.Exists(temporaryArchive))
                {
                    File.Delete(temporaryArchive);
                }
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            lb1_del();
        }

        void lb1_del()
        {

            if (!(lb2.SelectedIndex > -1)) { MessageBox.Show("Сначала выберите категорию"); return; }
            if (!(lb1.SelectedIndex > -1)) { MessageBox.Show("Сначала выберите объект для редактирования"); return; }

            var file = lb1.SelectedValue as Models3d;
            if (MessageBox.Show(
         "Вы действительно хотите удалить \"" + file.name + "\"?", "",
         MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                //  BD.conn("DELETE FROM `import3ds` WHERE id=" + file.id);
                //   MessageBox.Show("Объект успешно удалён");

                Directory.Delete(file.pathcategory, true);
                lb1_napolnenie();

            }



        }

        private void Lb3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            log.Add("чандж");
            UpdateServerDeleteMenuVisibility();
            lb4_napolnenie();
        }

        private void lb2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lb2.SelectedIndex > -1)
            {
                lb1_napolnenie();
            }
            else
            {
                if (localModelsLoadCancellation != null)
                {
                    localModelsLoadCancellation.Cancel();
                }
                lb1.ItemsSource = null;
            }

            UpdateActionStates();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            string otv = new dialuni().Show("Отмена", null, "Создать", "Добавление новой категории", "Укажите название новой категории", 3, 170);
            if (otv != "CANCEL")
            {
                string categoryName = (otv ?? string.Empty).Trim();
                if (categoryName.Length == 0 || categoryName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0
                    || categoryName == "." || categoryName == "..")
                {
                    MessageBox.Show("Название категории содержит недопустимые символы.",
                        "Новая категория", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Directory.CreateDirectory(Path.Combine(dir3ds_user, categoryName));
                lb2napolnene();
                treespis createdCategory = lb2.Items.Cast<treespis>()
                    .FirstOrDefault(delegate(treespis item) { return item.name == categoryName; });
                lb2.SelectedItem = createdCategory;
            }

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!(lb2.SelectedIndex > -1)) { MessageBox.Show("Выберите категорию для удаления"); return; }

            var Treespis = (lb2.SelectedItem as treespis);
            if (Treespis != null && Treespis.is_all)
            {
                MessageBox.Show("Системную категорию «Все» удалить нельзя.");
                return;
            }
            if (Directory.GetDirectories(dir3ds_user + "\\" + Treespis.name, "*", SearchOption.TopDirectoryOnly).Length > 0)
            { MessageBox.Show("Сначала удалите все модели, удалить категорию нельзя, пока в ней существуют модели"); return; }
            if (MessageBox.Show(
   "Вы действительно хотите удалить категорию \"" + Treespis.name + "\"?", "Удаление категории",
   MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Directory.Delete(dir3ds_user + "\\" + Treespis.name);
                lb2napolnene();
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (serverModelsLoadCancellation != null)
            {
                serverModelsLoadCancellation.Cancel();
            }
            if (localModelsLoadCancellation != null)
            {
                localModelsLoadCancellation.Cancel();
            }
            if (actionCancellation != null)
            {
                actionCancellation.Cancel();
            }

            kommunikacii_Set ps = kommunikacii_Set.Default;
            ps.Top = this.Top;
            ps.Left = this.Left;


            if (this.WindowState == WindowState.Maximized)
            {
                ps.SizeToContent = 1;
            }
            else
            {
                ps.SizeToContent = 0;
                ps.Width = this.Width;
                ps.Height = this.Height;
            }
            ps.lb1sel = lb1.SelectedIndex;
            ps.lb2sel = lb2.SelectedIndex;
            ps.lb4sel = lb4.SelectedIndex;
            ps.tab1sel = tab1.SelectedIndex;


            ps.Save();
        }
        void loadpage()
        {
            try
            {
                kommunikacii_Set ps = kommunikacii_Set.Default;

                if (ps.Top == -100 || !IsSavedWindowPositionVisible(ps.Left, ps.Top, ps.Width, ps.Height))
                {
                    this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

                }
                else
                {
                    this.Top = ps.Top;
                    this.Left = ps.Left;
                }
                if (ps.SizeToContent == 1)
                {
                    this.WindowState = WindowState.Maximized;
                }
                else
                {
                    this.Width = ps.Width;
                    this.Height = ps.Height;
                }

                tab1.SelectedIndex = ps.tab1sel;
                if (ps.tab1sel == 0)
                {
                    // lb3.SelectedIndex = ps.lb3sel;
                    //    lb4.Visibility = Visibility.Visible;
                    //    stpan.Visibility = Visibility.Visible;
                }
                else
                {
                    lb2.SelectedIndex = ps.lb2sel;
                    //   lb1.Visibility = Visibility.Visible;
                    //    btnserv.Visibility = Visibility.Visible;
                }

                lb1_selitem = ps.lb1sel;
                lb4_selitem = ps.lb4sel;
            }
            catch
            {

            }



        }

        private static bool IsSavedWindowPositionVisible(double left, double top, double width, double height)
        {
            if (double.IsNaN(left) || double.IsNaN(top) || double.IsInfinity(left) || double.IsInfinity(top))
            {
                return false;
            }

            double effectiveWidth = width > 0 ? width : 760;
            double effectiveHeight = height > 0 ? height : 500;
            Rect savedBounds = new Rect(left, top, effectiveWidth, effectiveHeight);
            Rect virtualScreen = new Rect(SystemParameters.VirtualScreenLeft, SystemParameters.VirtualScreenTop,
                SystemParameters.VirtualScreenWidth, SystemParameters.VirtualScreenHeight);
            Rect intersection = Rect.Intersect(savedBounds, virtualScreen);
            return !intersection.IsEmpty && intersection.Width >= 100 && intersection.Height >= 60;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            lb1_redak();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            lb1_del();
        }

        private void lb1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            DependencyObject source = e.OriginalSource as DependencyObject;
            ListBoxItem item = listBox == null || source == null
                ? null
                : ItemsControl.ContainerFromElement(listBox, source) as ListBoxItem;

            if (e.ChangedButton == MouseButton.Left && item != null)
            {
                item.IsSelected = true;
                Button_Click_1(sender, e);
            }
        }




        private void PolygonShapesMenu_OnClick(object sender, RoutedEventArgs e)
        {



            string idmodel_peremech = (lb1.SelectedItem as Models3d).pathcategory;//из
            string idcategor = dir3ds_user + "\\" + ((sender as System.Windows.Controls.MenuItem).Header as treespis).name + "\\" + (lb1.SelectedItem as Models3d).name;//куда
            if (idcategor.Equals(idmodel_peremech)) return;
            Directory.Move(idmodel_peremech, idcategor);
            lb1.UnselectAll();
            //    MessageBox.Show("asd");
            lb1_napolnenie();

        }

        private async void peremestitb_serv_event(treespis selcat)
        {
            Models3d file = lb4.SelectedValue as Models3d;
            if (selcat == null || file == null)
            {
                MessageBox.Show("Сначала выберите объект для перемещения.");
                return;
            }

            try
            {
                SetBusy(true, "Перемещение объекта…");
                string idcategor = selcat.id_server ?? string.Empty;
                string idmodel_peremech = file.id_server ?? string.Empty;
                string parameters = "id=" + Uri.EscapeDataString(idmodel_peremech)
                    + "&category=" + Uri.EscapeDataString(idcategor);
                await Task.Run(delegate { return (new web_zapros()).load("peremesh", parameters); });
                BD.execute("UPDATE `import3ds_server` SET  `category`='" + idcategor + "' WHERE id_server='" + file.id_server + "'");
                SetBusy(false, "Объект перемещён");
                MessageBox.Show("Объект успешно перемещён");
                lb4_napolnenie();
            }
            catch (Exception err)
            {
                log.Add("Ошибка перемещения объекта: " + err.Message);
                SetBusy(false, "Ошибка перемещения объекта");
                MessageBox.Show("Не удалось переместить объект.\n" + err.Message,
                    "3D объекты", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //добавление новой категории на сервер
        private async void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {



            string otv = new dialuni().Show("Отмена", null, "Создать", "Добавление новой категории НА СЕРВЕР", "Укажите название новой категории", 3, 170);
            if (otv != "CANCEL")
            {
                try
                {
                    SetBusy(true, "Создание категории…");
                    string parameters = "nazv=" + Uri.EscapeDataString(otv ?? string.Empty);
                    await Task.Run(delegate { return (new web_zapros()).load("create_categ", parameters); });
                    MessageBox.Show("Категория успешно создана");
                    await LoadServerCategoriesAsync();
                }
                catch (Exception ex)
                {
                    log.Add("Ошибка создания категории: " + ex.Message);
                    MessageBox.Show("Не удалось создать категорию.\n" + ex.Message,
                        "3D объекты", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    SetBusy(false, "Готово");
                }
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            //подгрузить данные с сервера

            treespis Treespis = lb3.SelectedItem as treespis;
            var json = (new web_zapros()).load("load_object", "category=" + Treespis.id_server);
            List<Models3d> server_load_obj = JsonConvert.DeserializeObject<List<Models3d>>(json);

            //  MessageBox.Show(Treespis.id_server.ToString());
            //  List<treespis> Result = server_load.Where(a => server.Contains(a.name)).ToList();

            List<Models3d> result = new List<Models3d>();


            ///тест









            for (int i = 0; i < server_load_obj.Count(); i++)
            {
                bool estb = true;
                for (int y = 0; y < server_obj.Count(); y++)
                {

                    if (server_load_obj[i].id_server == server_obj[y].id_server)
                    {

                        estb = false;
                    }
                }
                string new_ = "0";
                if (estb)
                {
                    new_ = "1";
                }


                string path_copy = dir3ds + Treespis.name + @"\";
                Directory.CreateDirectory(path_copy);//создадим папку для weб

                //  MessageBox.Show(path_copy + server_load_obj[i].path);
                string path3ds = path_copy + server_load_obj[i].path;
                string pathjpg = "";
                string pathjpgugo = "";

                if (server_load_obj[i].jpg_path != "")
                {

                    pathjpg = path_copy + server_load_obj[i].jpg_path;
                }
                if (server_load_obj[i].jpg_ugo != "")
                {
                    pathjpgugo = path_copy + server_load_obj[i].jpg_ugo;
                }

                string ecad = "https://ecad.giulianovars.ru/3dmodels/NEW/";


                if (!(File.Exists(pathjpg) || pathjpg == ""))
                {

                    (new WebClient()).DownloadFile(ecad + server_load_obj[i].jpg_path, pathjpg);//скачиваем картинку

                }




                result.Add(new Models3d
                {
                    id = server_load_obj[i].id,
                    path = server_load_obj[i].path,
                    name = server_load_obj[i].name,
                    jpg_path = server_load_obj[i].jpg_path,
                    jpg_ugo = server_load_obj[i].jpg_ugo,
                    x = server_load_obj[i].x,
                    y = server_load_obj[i].y,
                    z = server_load_obj[i].z,
                    category = server_load_obj[i].category,
                    id_server = server_load_obj[i].id_server,
                    loc_3ds = path3ds,
                    loc_jpg = pathjpg,
                    loc_ugojpg = pathjpgugo,
                    polzgroup = server_load_obj[i].polzgroup,
                    new_ = new_


                });

            }


            //    MessageBox.Show(result.Count().ToString());


            //  BD.conn("UPDATE `import3ds_server` SET  `new`='0' ");
            BD.execute("DELETE FROM `import3ds_server` WHERE category='" + Treespis.id_server + "'");
            foreach (Models3d elem in result)
            {
                BD.execute("INSERT INTO  `import3ds_server` (id_server, nazv,path3ds,pathjpg,pathjpgugo,x,y,z,category,new, loc_3ds, loc_jpg, loc_ugojpg, polzgroup)  VALUES ('" +
                    elem.id_server + "', '" +
                    elem.name + "', '" +
                    elem.path + "', '" +
                    elem.jpg_path + "', '" +
                    elem.jpg_ugo + "', '" +
                    elem.x + "', '" +
                    elem.y + "', '" +
                    elem.z + "', '" +
                    elem.category + "', '" +
                    elem.new_ + "', '" +
                    elem.loc_3ds + "','" +
                    elem.loc_jpg + "','" +
                    elem.loc_ugojpg + "','" +
                    elem.polzgroup + "')");

            }
            MessageBox.Show("Данные успешно обновлены");
            lb4_napolnenie();


        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {

            var model = (lb3.SelectedItem as treespis);

            if (MessageBox.Show(
"Вы действительно хотите удалить \"" + model.name + "\"?", "",
MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {



                var json = (new web_zapros()).load("delete_group", "delidcategor=" + model.id);



                if (json != "FAIL")
                {
                    MessageBox.Show("Категория успешно удалена");
                    lb3napolnene();

                }
                else
                {
                    MessageBox.Show("Сначала удалите все модели, удалить категорию нельзя, пока в ней существуют модели");
                }
            }


        }

        private treespis GetSelectedServerCategory()
        {
            return lb3.SelectedItem as treespis;
        }

        private bool CanDeleteSelectedServerObject()
        {
            treespis category = GetSelectedServerCategory();
            return admin == "1"
                && category != null
                && string.Equals(category.name, "!Удалить!", StringComparison.Ordinal);
        }

        private void UpdateServerDeleteMenuVisibility()
        {
            Visibility visibility = CanDeleteSelectedServerObject()
                ? Visibility.Visible
                : Visibility.Collapsed;

            delete_server_object.Visibility = visibility;
            delete_server_object_separator.Visibility = visibility;
        }

        private async void MenuItem_Click_5(object sender, RoutedEventArgs e)//удалить объект с сервера
        {
            if (!CanDeleteSelectedServerObject())
            {
                MessageBox.Show("Удалять объекты с сервера можно только администратору из папки !Удалить!.",
                    "Удаление объекта", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Models3d file = lb4.SelectedValue as Models3d;
            if (file == null)
            {
                MessageBox.Show("Сначала выберите объект для удаления.", "Удаление объекта");
                return;
            }

            if (string.IsNullOrWhiteSpace(file.id_server))
            {
                MessageBox.Show("Сервер не передал идентификатор выбранного объекта.",
                    "Удаление объекта", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show(
                "Вы действительно хотите безвозвратно удалить с сервера \"" + file.name + "\"?",
                "Удаление объекта", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
            {
                return;
            }

            string result;
            try
            {
                result = await Task.Run(() => DeleteServerObject(file.id_server));
            }
            catch (Exception ex)
            {
                log.Add("Ошибка удаления объекта с сервера: " + ex.Message);
                MessageBox.Show("Не удалось связаться с сервером. Объект не удалён.",
                    "Удаление объекта", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.Equals(result.Trim(), "FAIL", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Не удалось удалить объект с сервера.",
                    "Удаление объекта", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("Объект успешно удалён с сервера.", "Удаление объекта");
            lb4_napolnenie();
        }

        private static string DeleteServerObject(string id)
        {
            string url = "https://ecad.giulianovars.ru/php/3dsobject/3ds.php?command=delete_obj&id="
                + Uri.EscapeDataString(id);

            using (WebClient client = new WebClient())
            {
                byte[] response = client.DownloadData(url);
                return Encoding.UTF8.GetString(response);
            }
        }

        //
        private void lb4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lb4.SelectedItem != null)
            {
                var file = lb4.SelectedValue as Models3d;
                label1.Content = "Группа у пользователя - " + file.polzgroup;
                lb4_selitem = lb4.SelectedIndex;
            }
            else
            {
                label1.Content = lb3.SelectedItem == null ? "Выберите категорию" : "Выберите объект";
            }

            UpdateActionStates();
        }

        async Task UploadFileAsync(string url, string filePath)
        {
            using (FileStream fileStream = File.OpenRead(filePath))
            using (var content = new MultipartFormDataContent())
            using (var fileContent = new StreamContent(fileStream))
            {
                fileContent.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/octet-stream");
                content.Add(fileContent, "file", Path.GetFileName(filePath));

                using (HttpResponseMessage response = await httpClient.PostAsync(url, content))
                {
                    response.EnsureSuccessStatusCode();
                    string responseString = await response.Content.ReadAsStringAsync();
                    log.Add("Upload successful: " + responseString);
                }
            }
        }
        private async void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            treespis category = lb2.SelectedItem as treespis;
            Models3d model = lb1.SelectedItem as Models3d;
            if (category == null)
            {
                MessageBox.Show("Сначала выберите категорию.");
                return;
            }
            if (category.is_all)
            {
                MessageBox.Show("Для отправки выберите конкретную категорию.");
                return;
            }
            if (model == null)
            {
                MessageBox.Show("Сначала выберите объект.");
                return;
            }
            if (!File.Exists(model.path))
            {
                MessageBox.Show("3DS-файл на компьютере не обнаружен.");
                return;
            }
            if (MessageBox.Show("Отправить \"" + model.name + "\" на сервер?", "Отправка объекта",
                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            actionCancellation = new CancellationTokenSource();
            try
            {
                SetBusy(true, "Отправка объекта " + model.name + "…");
                await UploadSingleUserModelAsync(model, category, actionCancellation.Token);
                SetBusy(false, "Объект успешно отправлен");
                MessageBox.Show("Объект успешно отправлен.", "Отправка объекта");
            }
            catch (OperationCanceledException)
            {
                SetBusy(false, "Отправка отменена");
            }
            catch (Exception ex)
            {
                log.Add("Ошибка отправки объекта: " + ex.Message);
                SetBusy(false, "Ошибка отправки объекта");
                MessageBox.Show("Не удалось отправить объект.\n" + ex.Message,
                    "Отправка объекта", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (actionCancellation != null)
                {
                    actionCancellation.Dispose();
                    actionCancellation = null;
                }
            }
        }

        private async Task UploadSingleUserModelAsync(Models3d model, treespis category,
            CancellationToken cancellationToken)
        {
            string time = DateTime.Now.ToFileTimeUtc().ToString();
            string safeName = Tr2(model.name).Replace(' ', '_');
            if (string.IsNullOrWhiteSpace(safeName))
            {
                safeName = "model";
            }

            string archiveName = safeName + "_" + time + ".zip";
            string previewName = safeName + "_" + time + ".jpg";
            string archivePath = Path.Combine(Path.GetTempPath(), archiveName);
            string previewPath = Path.Combine(Path.GetTempPath(), previewName);

            try
            {
                await Task.Run(delegate
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    if (File.Exists(archivePath)) File.Delete(archivePath);
                    ZipFile.CreateFromDirectory(model.pathcategory, archivePath);
                    if (File.Exists(model.jpg_path))
                    {
                        if (File.Exists(previewPath)) File.Delete(previewPath);
                        File.Copy(model.jpg_path, previewPath);
                    }
                }, cancellationToken);

                await UploadFileAsync("https://ecad.giulianovars.ru/3dmodels/upload3d.php", archivePath);
                cancellationToken.ThrowIfCancellationRequested();
                if (File.Exists(previewPath))
                {
                    await UploadFileAsync("https://ecad.giulianovars.ru/3dmodels/upload3d.php", previewPath);
                }

                INIManager manager = new INIManager(GetEcadProIni());
                string activationKey = manager.GetPrivateString("giulianovars", "attivazione");
                string parameters = "filename=" + Uri.EscapeDataString(model.name ?? string.Empty)
                    + "&filename3ds=" + Uri.EscapeDataString(archiveName)
                    + "&filenamejpg=" + Uri.EscapeDataString(previewName)
                    + "&idclienta=" + Uri.EscapeDataString(activationKey ?? string.Empty)
                    + "&x=" + Uri.EscapeDataString(model.x ?? string.Empty)
                    + "&y=" + Uri.EscapeDataString(model.y ?? string.Empty)
                    + "&z=" + Uri.EscapeDataString(model.z ?? string.Empty)
                    + "&categoryuser=" + Uri.EscapeDataString(category.name ?? string.Empty);
                await Task.Run(delegate { return (new web_zapros()).load("new3ds", parameters); },
                    cancellationToken);
            }
            finally
            {
                if (File.Exists(archivePath)) File.Delete(archivePath);
                if (File.Exists(previewPath)) File.Delete(previewPath);
            }
        }

        string GetEcadProIni()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\ecadpro.ini";
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            dob_new3ds();
        }








        public static string loadsinch(string comand, string param = "")
        {
            string otvet = "";
            try
            {
                if (param != "")
                {
                    param = "&" + param;
                }
                //загрузка дынных для сервера
                log.Add("/загрузка дынных для сервера");
                WebClient client = new WebClient();
                
                string url = "https://ecad.giulianovars.ru/php/3dsobject/3ds.php?command=" + comand + param;
                log.Add(url);
                byte[] response = client.DownloadData(url);
                otvet = Encoding.UTF8.GetString(response);
                log.Add("успех");
            }
            catch (Exception err)
            {
                log.Add("Ошибка запроса к серверу: " + err.Message);
            }
            return otvet;
        }






        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(
              "Вы действительно хотите очистить локальную базу данных?", "",
              MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                BD.execute("DELETE FROM `import3ds_category_server` ");
                BD.execute("DELETE FROM `import3ds_server` ");
                lb3napolnene();
                lb4.ItemsSource = null;
            }
        }

        private void move_to_new_41()
        {

            string pathcategory = Environment.CurrentDirectory + @"\3dsObject\User";
            string[] dirs = Directory.GetDirectories(pathcategory);

            bool msgpokaz = true;

            foreach (string dir in dirs)
            {

                foreach (string file3ds in Directory.GetFiles(dir, "*.3ds", SearchOption.TopDirectoryOnly))
                {
                    if (msgpokaz)
                    {
                        MessageBox.Show("Восстановление старых объектов, нажмите ОК и подождите до завершения");
                        msgpokaz = false;
                    }
                    string namefile = file3ds.Split('\\').Last().Split('_').First();
                    string path3ds_new = dir + @"\" + namefile + @"\" + namefile + ".3ds";
                    string pathjpg_new = dir + @"\" + namefile + @"\teh\prev.jpg";

                    Directory.CreateDirectory(dir + @"\" + namefile + @"\teh");

                    if (File.Exists(path3ds_new)) File.Delete(path3ds_new);
                    File.Move(file3ds, path3ds_new);
                    string pathjpg = file3ds.Replace(".3ds", ".jpg");
                    if (File.Exists(pathjpg))
                    {

                        if (File.Exists(pathjpg_new)) File.Delete(pathjpg_new);
                        File.Move(pathjpg, pathjpg_new);
                    }
                }
            }

            if (msgpokaz == false) MessageBox.Show("Готово");



            //   MessageBox.Show("Готово");
            //   lb2napolnene();
        }

        private async void Button_Click_7(object sender, RoutedEventArgs e)
        {
            treespis selectedCategory = lb2.SelectedItem as treespis;
            IEnumerable<Models3d> source = lb1.ItemsSource as IEnumerable<Models3d>;
            List<Models3d> models = source == null ? new List<Models3d>() : source.ToList();

            if (selectedCategory == null)
            {
                MessageBox.Show("Сначала выберите категорию.");
                return;
            }
            if (selectedCategory.is_all)
            {
                MessageBox.Show("Для массовой отправки выберите конкретную категорию.");
                return;
            }
            if (models.Count == 0)
            {
                MessageBox.Show("В выбранной категории нет объектов.");
                return;
            }
            if (MessageBox.Show("Отправить всю категорию на сервер?", "Отправка объектов",
                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            actionCancellation = new CancellationTokenSource();
            try
            {
                SetBusy(true, "Подготовка отправки…");
                INIManager manager = new INIManager(GetEcadProIni());
                string activationKey = manager.GetPrivateString("giulianovars", "attivazione");

                for (int index = 0; index < models.Count; index++)
                {
                    Models3d model = models[index];
                    actionCancellation.Token.ThrowIfCancellationRequested();
                    if (!File.Exists(model.path))
                    {
                        throw new FileNotFoundException("3DS-файл не найден: " + model.name, model.path);
                    }

                    statusText.Text = "Отправка " + (index + 1) + " из " + models.Count + ": " + model.name;
                    string temporaryZip = Path.Combine(Path.GetTempPath(),
                        "3ds_to_server_TreeCadN_" + Guid.NewGuid().ToString("N") + ".zip");

                    try
                    {
                        string time = DateTime.Now.ToFileTimeUtc().ToString();
                        string safeName = Tr2(model.name).Replace(' ', '_');
                        if (string.IsNullOrWhiteSpace(safeName))
                        {
                            safeName = "model";
                        }
                        string archiveName = safeName + "_" + time + ".zip";
                        string previewName = safeName + "_" + time + ".jpg";

                        await Task.Run(delegate
                        {
                            actionCancellation.Token.ThrowIfCancellationRequested();
                            ZipFile.CreateFromDirectory(model.pathcategory, temporaryZip);
                            (new FTP()).otprav(temporaryZip,
                                "ftp://ecad.giulianovars.ru/public/3dmodels/NEW/" + archiveName);
                            if (File.Exists(model.jpg_path))
                            {
                                (new FTP()).otprav(model.jpg_path,
                                    "ftp://ecad.giulianovars.ru/public/3dmodels/NEW/" + previewName);
                            }
                        }, actionCancellation.Token);

                        string parameters = "filename=" + Uri.EscapeDataString(model.name ?? string.Empty)
                            + "&filename3ds=" + Uri.EscapeDataString(archiveName)
                            + "&filenamejpg=" + Uri.EscapeDataString(previewName)
                            + "&idclienta=" + Uri.EscapeDataString(activationKey ?? string.Empty)
                            + "&x=" + Uri.EscapeDataString(model.x ?? string.Empty)
                            + "&y=" + Uri.EscapeDataString(model.y ?? string.Empty)
                            + "&z=" + Uri.EscapeDataString(model.z ?? string.Empty)
                            + "&categoryuser=" + Uri.EscapeDataString(selectedCategory.name ?? string.Empty);
                        await Task.Run(delegate { return (new web_zapros()).load("new3ds", parameters); },
                            actionCancellation.Token);
                    }
                    finally
                    {
                        if (File.Exists(temporaryZip))
                        {
                            File.Delete(temporaryZip);
                        }
                    }
                }

                MessageBox.Show("Отправлено объектов: " + models.Count, "Отправка объектов");
                SetBusy(false, "Отправлено объектов: " + models.Count);
            }
            catch (OperationCanceledException)
            {
                SetBusy(false, "Отправка отменена");
            }
            catch (Exception ex)
            {
                log.Add("Ошибка отправки категории: " + ex.Message);
                SetBusy(false, "Ошибка отправки категории");
                MessageBox.Show("Не удалось отправить категорию.\n" + ex.Message,
                    "Отправка объектов", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (actionCancellation != null)
                {
                    actionCancellation.Dispose();
                    actionCancellation = null;
                }
            }
        }

#if false
        private void Button_Click_7_legacy(object sender, RoutedEventArgs e)
        {
            //отправить все на фабрику





            if (!(lb2.SelectedIndex > -1)) { MessageBox.Show("Сначала выберите категорию"); return; }
            //       if (!(lb1.SelectedIndex > -1)) { MessageBox.Show("Сначала выберите объект"); return; }


            if (!(MessageBox.Show(
"Вы действительно хотите отправить всю категорию на сервер?", "",
MessageBoxButton.YesNo) == MessageBoxResult.Yes)) return;

            foreach (Models3d file in lb1.Items)
            {





                if (!(File.Exists(file.path))) { MessageBox.Show("3ds файл на Вашем компьютере не обнаружен"); return; }

                string startPath = file.pathcategory;
                string zipPath = Path.GetTempPath() + "3ds_to_server_TreeCadN.zip";

                if (File.Exists(zipPath)) File.Delete(zipPath);
                log.Add("zipPath - " + zipPath);
                ZipFile.CreateFromDirectory(startPath, zipPath);

                string time = DateTime.Now.ToFileTimeUtc().ToString();
                string path3ds = Tr2(file.name).Replace(' ', '_') + "_" + time + ".zip";
                string pathjpg = Tr2(file.name).Replace(' ', '_') + "_" + time + ".jpg";


                log.Add("Отправляем заказ на фабрику: " + path3ds + "  " + pathjpg);


                (new FTP()).otprav(zipPath, "ftp://ecad.giulianovars.ru/public/3dmodels/NEW/" + path3ds);
                if (File.Exists(file.jpg_path))
                {
                    (new FTP()).otprav(file.jpg_path, "ftp://ecad.giulianovars.ru/public/3dmodels/NEW/" + pathjpg);
                }


                string categoryuser = (lb2.SelectedItem as treespis).name;


                //Создание объекта, для работы с файлом
                INIManager manager = new INIManager(GetEcadProIni());
                string authotiz_root = manager.GetPrivateString("giulianovars", "attivazione");//получ ключ активации



                (new web_zapros()).load("new3ds",
                    "filename=" + file.name +
                    "&filename3ds=" + path3ds +
                    "&filenamejpg=" + pathjpg +
                    "&idclienta=" + authotiz_root +
                    "&x=" + file.x +
                    "&y=" + file.y +
                    "&z=" + file.z +
                    "&categoryuser=" + categoryuser);
             //   MessageBox.Show("Объект успешно отправлен");






            }
            MessageBox.Show("Объект успешно отправлен");










        }

#endif
        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            lb1_redak();
        }



        private void tab1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ReferenceEquals(e.Source, tab1))
            {
                return;
            }

            if (item2 != null && item2.IsSelected && lb1 != null && lb1.SelectedItem is Models3d)
            {
                SetSelectedLocalModel(lb1.SelectedItem as Models3d);
            }
            else
            {
                selectedModel = string.Empty;
            }

            UpdateActionStates();
        }
        private static string Tr2(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return string.Empty;
            }

            StringBuilder ret = new StringBuilder();
            string rus = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
            string[] eng = { "A", "B", "V", "G", "D", "E", "E", "ZH", "Z", "I", "Y",
                "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "F", "KH", "TS",
                "CH", "SH", "SHCH", "", "Y", "", "E", "YU", "YA" };

            foreach (char character in s)
            {
                char upper = char.ToUpperInvariant(character);
                int index = rus.IndexOf(upper);
                if (index >= 0)
                {
                    ret.Append(eng[index]);
                }
                else if ((character >= 'A' && character <= 'Z')
                    || (character >= 'a' && character <= 'z')
                    || char.IsDigit(character))
                {
                    ret.Append(character);
                }
                else if (char.IsWhiteSpace(character) || character == '-' || character == '_')
                {
                    ret.Append('_');
                }
            }

            return ret.ToString();
        }

    }

    public class dialuni
    {
        public string otvet = "";
        public string Show(string sbtn1, string sbtn2, string sbtn3, string stitle, string stext, int param, double heightwin)
        {
            dialog_univi dial = new dialog_univi(this, sbtn1, sbtn2, sbtn3, stitle, stext, param, heightwin);
            dial.ShowDialog();

            return otvet;
        }


    }
    public class Models3d
    {

        public string path { get; set; }
        public string name { get; set; }
        public string jpg_path { get; set; }
        public string jpg_ugo { get; set; }
        public string x { get; set; }
        public string y { get; set; }
        public string z { get; set; }
        public string id { get; set; }
        public string category { get; set; }
        public string id_server { get; set; }
        public string loc_3ds { get; set; }
        public string loc_jpg { get; set; }
        public string loc_ugojpg { get; set; }
        public string zvetbg { get; set; }
        public string download { get; set; }
        public string polzgroup { get; set; }
        public string new_ { get; set; }
        public string pathcategory { get; set; }
        public BitmapImage bitmap_prev { get; set; }
        public BitmapImage jpgsource { get; set; }

    }


    public class Models3dJSON
    {

        public string x { get; set; }
        public string y { get; set; }
        public string z { get; set; }

        public Models3dJSON()
        {
        }
        public Models3dJSON(string x, string y, string z)
        {
            this.x = x;
            this.y = y;
            this.z = z;

        }


    }

    public class treespis
    {

        //     public string path { get; set; }
        public string path_dir { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public string id_server { get; set; }
        //   public string type { get; set; }
        public string zvetbg { get; set; }

        public string owner { get; set; }
        [JsonIgnore]
        public bool is_all { get; set; }
        public ObservableCollection<treespis> Nodes { get; set; }
    }
    public class FTP
    {

        public void otprav(string filename, string path)
        {
            //ftp://ecad.giulianovars.ru/public/
            FileInfo toUpload = new FileInfo(filename);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(path);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential("ecad_ftp", "bFqeNo4Xp2");
            Stream ftpStream = request.GetRequestStream();
            FileStream fileStream = File.OpenRead(filename);
            byte[] buffer = new byte[1024];
            int bytesRead = 0;
            do
            {
                bytesRead = fileStream.Read(buffer, 0, 1024);
                ftpStream.Write(buffer, 0, bytesRead);
            }
            while (bytesRead != 0);
            fileStream.Close();
            ftpStream.Close();
        }

    }
    class web_zapros
    {
        //вызов
        //(new web_zapros()).load("new3ds","asdasdsa");
        public string load(string comand, string param = "")
        {
            string otvet = "";
            try
            {
                if (param != "")
                {
                    param = "&" + param;
                }
                //загрузка дынных для сервера
                WebClient client = new WebClient();
                string url = "https://ecad.giulianovars.ru/php/3dsobject/3ds.php?command=" + comand + param;
                byte[] response = client.DownloadData(url);
                otvet = System.Text.Encoding.UTF8.GetString(response);

            }
            catch (Exception err)
            {
                log.Add("Ошибка запроса к серверу: " + err.Message);
            }
            return otvet;
        }





    }



}
