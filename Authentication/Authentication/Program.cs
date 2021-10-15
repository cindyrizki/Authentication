using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Authentication
{
    class Program : Akun
    {
        public static int indexSameUser = 0;
        public static void Main(string[] args)
        {
            int exit = 0;
            List<Akun> akun = new List<Akun>();

            do
            {
                try
                {
                    int pilih;
                    Console.Clear();
                    Menu(akun);
                    pilih = int.Parse(Console.ReadLine());

                    switch (pilih)
                    {
                        case 1:
                            Console.Clear();
                            CreateUser(akun);
                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 2:
                            Console.Clear();
                            ShowUser(akun);
                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 3:
                            Console.Clear();
                            Update(akun);
                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 4:
                            Console.Clear();
                            Search(akun);
                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 5:
                            Console.Clear();
                            Login(akun);
                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 6:
                            Console.Clear();
                            DeleteUser(akun);
                            Console.ReadLine();
                            Console.Clear();
                            break;
                        case 7:
                            exit = 7;
                            Console.WriteLine();
                            Console.WriteLine("Program Basic Authentication Selesai");
                            break;
                        default:
                            Console.WriteLine();
                            Console.WriteLine("Masukan Anda tidak valid");
                            Console.ReadLine();
                            Console.Clear();
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Masukan Anda tidak valid");
                    Console.ReadLine();
                    Console.Clear();
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Masukan Anda tidak valid");
                    Console.ReadLine();
                    Console.Clear();
                }
            } while (exit != 7);
        }

        public static void CreateUser(List<Akun> akun)
        {
            Console.WriteLine("===================================");
            Console.WriteLine("\tCREATE USER");
            Console.WriteLine("===================================");
            bool cekFirst = false;
            string firstName, lastName;
            string password, hashPassword;

            do
            {
                Console.Write("First Name : ");
                firstName = Console.ReadLine();

                Console.Write("Last Name : ");
                lastName = Console.ReadLine();

                if (firstName == "" || lastName == "")
                {

                    Console.WriteLine("Input Data tidak sesuai");
                    Console.WriteLine();
                }
                else
                {
                    cekFirst = true;
                }
            } while (cekFirst == false);

            bool cek = false;

            do
            {
                Console.Write("Password (min. 6 karakter): ");
                password = (Console.ReadLine());

                Regex rgx = new Regex(@"^.*(?=.{6,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$", RegexOptions.Compiled);


                if (password == "")
                {
                    Console.WriteLine("Harap isikan password");
                }
                else if (!rgx.IsMatch(password))
                {

                    Console.WriteLine("Password harus mengandung angka, huruf kapital, dan simbol");
                    Console.WriteLine();
                    Console.WriteLine("Harap isi password kembali");

                }
                else
                {
                    cek = true;
                }
            } while (cek == false);


            string userName = (firstName.Substring(0, 2) + lastName.Substring(0, 2));
            bool exist = akun.Exists(item => item.Uname == userName);

            if (exist)
            {

                userName = string.Concat(userName, indexSameUser++);
            }

            hashPassword = BCrypt.Net.BCrypt.HashPassword(password);
            akun.Add(new Akun(firstName, lastName, userName, hashPassword));
            Console.WriteLine("Data berhasil dibuat");

        }

        public static void ShowUser(List<Akun> akun)
        {
            Console.WriteLine("===================================");
            Console.WriteLine("\tUSER");
            Console.WriteLine("===================================");

            foreach (var akun1 in akun)
            {
                Console.WriteLine("-----------------------------");
                Console.WriteLine($"Nama        : {akun1.firstName} {akun1.lastName}");
                Console.WriteLine($"Username    : {akun1.Uname}");
                Console.WriteLine($"Password    : {akun1.Password}");
                Console.WriteLine("-----------------------------");

            }
        }

        public static void Search(List<Akun> akun)
        {
            Console.WriteLine("===================================");
            Console.WriteLine("\tSEARCH USER");
            Console.WriteLine("===================================");

            Console.WriteLine();
            Console.Write("Username : ");
            string searchUsername = Console.ReadLine();

            bool exist = akun.Exists(item => item.Uname == searchUsername);

            if (exist)
            {
                Console.WriteLine();
                Console.WriteLine("---------------------------------");
                Console.WriteLine("User telah terdaftar dengan nama");
                Console.WriteLine($"Nama : {akun.Find(user => user.Uname.Contains(searchUsername)).firstName} {akun.Find(user => user.Uname.Contains(searchUsername)).lastName}");
                Console.WriteLine("---------------------------------");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("User belum terdaftar");
                Console.ReadKey();
            }
        }

        public static void Login(List<Akun> akun)
        {

            Console.WriteLine("===================================");
            Console.WriteLine("\tLOGIN USER");
            Console.WriteLine("===================================");

            Console.WriteLine();
            Console.Write("Username : ");
            string login = Console.ReadLine();
            Console.Write("Password : ");
            string pass = Console.ReadLine();
            bool existAkun = akun.Exists(item => item.Uname == login);
            if (!existAkun)
            {
                Console.WriteLine("Username tidak terdaftar");
            }
            else
            {
                string hashPassword = akun.Find(user => user.Uname.Contains(login)).Password;
                bool validPassword = BCrypt.Net.BCrypt.Verify(pass, hashPassword);
                bool exist = akun.Exists(item => item.Uname == login && validPassword == true);

                if (exist)
                {
                    Console.WriteLine();
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine("Login Berhasil");
                    Console.WriteLine("---------------------------------");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Error : Username/Password salah");
                    Console.ReadKey();
                }
            }
        }

        public static void DeleteUser(List<Akun> akun)
        {
            Console.WriteLine("===================================");
            Console.WriteLine("\tDELETE USER");
            Console.WriteLine("===================================");
            ShowUser(akun);
            Console.WriteLine();
            Console.WriteLine("Isikan username yang ingin dihapus");
            Console.Write("Username : ");
            string username1 = Console.ReadLine();

            bool exist = akun.Exists(item => item.Uname == username1);

            if (exist)
            {
                Console.WriteLine("Apakah Anda sungguh ingin menghapus User ini (Y/N) ? ");
                if (Console.ReadKey().Key == ConsoleKey.Y)
                {
                    akun.RemoveAll(item => item.Uname == username1);
                    Console.WriteLine();
                    Console.WriteLine("Data User telah terhapus");
                }
            }
            else
            {
                Console.WriteLine("Username tidak terdaftar");
            }
        }

        public static void Update(List<Akun> akun)
        {
            Console.WriteLine("===================================");
            Console.WriteLine("\tUPDATE USER");
            Console.WriteLine("===================================");

            Console.WriteLine();
            Console.Write("Username : ");
            string username1 = Console.ReadLine();

            bool exist = akun.Exists(item => item.Uname == username1);
            int index = akun.FindIndex(item => item.Uname == username1);

            if (exist)
            {
                Console.WriteLine();
                Console.WriteLine("Input data baru");
                Console.WriteLine("-------------------");
                Console.Write("First Name : ");
                string firstName = Console.ReadLine();
                Console.Write("Last Name : ");
                string lastName = Console.ReadLine();
                bool cek = false;
                string password, hashPassword;

                do
                {
                    Console.Write("Password (min. 6 karakter): ");
                    password = (Console.ReadLine());

                    Regex rgx = new Regex(@"^.*(?=.{6,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$", RegexOptions.Compiled);


                    if (password == "")
                    {
                        Console.WriteLine("Harap isikan password");
                    }
                    else if (!rgx.IsMatch(password))
                    {

                        Console.WriteLine("Password harus mengandung angka, huruf kapital, dan simbol");
                        Console.WriteLine();
                        Console.WriteLine("Harap isi password kembali");
                    }
                    else
                    {
                        cek = true;
                    }
                } while (cek == false);

                string userName = (firstName.Substring(0, 2) + lastName.Substring(0, 2));
                hashPassword = BCrypt.Net.BCrypt.HashPassword(password);
                akun[index].firstName = firstName;
                akun[index].lastName = lastName;
                akun[index].Uname = userName;
                akun[index].Password = hashPassword;
                Console.WriteLine("Data telah berhasil diupdate");
            }
            else
            {
                Console.WriteLine("Username tidak terdaftar");
            }

        }

        public static void Menu(List<Akun> akun)
        {
            Console.WriteLine("==Basic Authentication==");
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. Show User");
            Console.WriteLine("3. Update User");
            Console.WriteLine("4. Search User");
            Console.WriteLine("5. Login");
            Console.WriteLine("6. Delete User");
            Console.WriteLine("7. Exit");
            Console.Write("Input : ");
        }

    }
}