namespace laba666
{

    //Котики
    class Meoweble : Meowww
    {
        private readonly string name; //можно инициал только констр
        public string Name { get => name; } //св-во предостав доступ к имени для чтения

        public Meoweble(string _name) //экземпляр с заданным именем
        {
            name = _name;
        }

        public void meow(int amount = 1)
        {
            if (amount < 1)
                return;
            Console.Write($"{name}: ");
            for (int i = 1; i < amount; ++i)
            { Console.Write("мяу-"); }
            Console.WriteLine("мяу!");
        }
    }


    public interface Meowww
    {
        public void meow(int amount = 1); //объявл

        public static void meowAll(Meowww[] meowables)
        {
            foreach (Meowww meowable in meowables) //для кваждого объекта meow
                meowable.meow();
        }
    }


    class MeowCount : Meowww
    {
        private readonly Meowww meowObj; // ссылка на объект, реализующий IMeow
        private int callCount; // счетчик вызовов метода meow
        public int Count { get => callCount; } // свойство для доступа к счетчику

        public MeowCount(Meowww _meowObj)
        {
            meowObj = _meowObj; // инициализация ссылки на объект
            callCount = 0; // инициализация счетчика
        }

        public void meow(int amount)
        {
            if (amount < 1) // проверка на корректность количества
                return;
            callCount += amount;
            meowObj.meow(amount); // вызов метода meow у оборачиваемого объекта
        }
    }

    class Puma : Meowww
    {
        private readonly string name;
        public string Name { get => name; }

        public Puma(string _name)
        {
            name = _name;
        }

        public void meow(int count = 1)
        {
            if (count < 1)
                return;
            Console.Write($"{name}: ");
            for (int i = 1; i < count; ++i)
            { Console.Write("раррр-"); }
            Console.WriteLine("раррр!");

        }
    }


    //----------------------------------------------------------------

    interface IFraction
    {
        public int Chislitel { set; }
        public int Znamenatel { set; }

        public double Value();
    }
    class FractionD : IFraction
    {
        private Fraction fraction; //хранит ссылку на экз класса Fraction
        private double cachedValue; //кэширует зн дроби

        public FractionD(Fraction _fraction)
        {
            fraction = _fraction;
            cachedValue = fraction.Value(); // Инициализируем кэшированное значение при создании
        }

        public int Chislitel
        {
            set
            {
                fraction.Chislitel = value;
                calcVal();
            }
        }

        public int Znamenatel
        {
            set
            {
                fraction.Znamenatel = value;
                calcVal();
            }
        }

        public double Value()
        {
            // Обновляем кэшированное значение 
            calcVal();
            return cachedValue;
        }

        private void calcVal()
        {
            cachedValue = fraction.Value(); //пересчитываем значение дроби только тогда, когда это необходимо
        }
    }

    class Fraction : IFraction, ICloneable
    {
        private int chislitel;
        private int znamenatel;
        public int Chislitel 
        { 
            get => chislitel; 
            set 
            { 
                chislitel = znamenatel; 
                simplify(); 
            } 
        }
        public int Znamenatel
        {
            get => znamenatel; set
            {
                if (value == 0) throw new Exception("0 не может быть знаменателем");
                if (value < 0)
                {
                    chislitel *= -1;
                    value *= -1;
                }
                znamenatel = value;
                simplify();
            }
        }

        public Fraction(int _chislitel, int _znamenatel)
        {
            if (_znamenatel == 0)
                throw new Exception("0 не может быть знаменателем");
            chislitel = _chislitel;
            if (_znamenatel < 0)
            {
                chislitel *= -1;
                _znamenatel *= -1;
            }
            znamenatel = _znamenatel;
            simplify();
        }

        public Fraction(int a)
        {
            chislitel = a;
            znamenatel = 1;
        }

        private void simplify()
        {
            int nod = NOD(Math.Abs(chislitel), znamenatel);
            chislitel /= nod;
            znamenatel /= nod;
        }

        private static int NOD(int a, int b)// НОД
        { 
            while (a != b && a != 0 && b != 0)
            {
                if (a > b)
                {
                    a %= b;
                }
                else b %= a;
            }

            return (a != 0) ? a : b; //если a != нулю, возвращается a, иначе возвращается b
        }

        private static int NOK(int a, int b)// НОК
        { 
            return a * b / NOD(a, b);
        }

        private Fraction reverse()
        {
            if (chislitel == 0)
            { 
                return this; 
            }
            if (chislitel < 0)
            {
                return new Fraction(-znamenatel, chislitel);
            }

            return new Fraction(znamenatel, chislitel);
        }

        public static Fraction operator +(Fraction fract1, Fraction fract2)
        {
            return new Fraction(fract1.chislitel * NOK(fract1.znamenatel, fract2.znamenatel) / fract1.znamenatel + fract2.chislitel * NOK(fract1.znamenatel, fract2.znamenatel) / fract2.znamenatel, NOK(fract1.znamenatel, fract2.znamenatel));
        }
        public static Fraction operator *(Fraction fract1, Fraction fract2)
        {
            return new Fraction(fract1.chislitel * fract2.chislitel, fract1.znamenatel * fract2.znamenatel);
        }
        public static Fraction operator -(Fraction fract1, Fraction fract2)
        {
            return new Fraction(fract1.chislitel * NOK(fract1.znamenatel, fract2.znamenatel) / fract1.znamenatel - fract2.chislitel * NOK(fract1.znamenatel, fract2.znamenatel) / fract2.znamenatel, NOK(fract1.znamenatel, fract2.znamenatel));
        }
        public static Fraction operator /(Fraction fract1, Fraction fract2)
        {
            return fract1 * fract2.reverse();
        }

        public static Fraction operator -(Fraction fract)
        {
            return -1 * fract;
        }

        public static implicit operator Fraction(int a)
        {
            return new Fraction(a);
        }

        public static bool operator ==(Fraction fract1, Fraction fract2)
        {
            return (fract1.chislitel == fract2.chislitel) && (fract1.znamenatel == fract2.znamenatel);
        }
        public static bool operator !=(Fraction fract1, Fraction fract2)
        {
            return !(fract1 == fract2);
        }

        public object Clone()
        {
            return new Fraction(chislitel, znamenatel);
        }

        public double Value()
        {
            if (znamenatel == 0)
                throw new DivideByZeroException("Знаменатель не может быть равен нулю.");

            return (double)chislitel / znamenatel;
        }

        public override string ToString()
        {
            return $"{chislitel}/{znamenatel}";
        }
    }
    //---------------------------------------------------------------
    internal class Program
    {


        static void Main(string[] args)
        {
            Meoweble barsik = new("Барсик");
            barsik.meow();
            barsik.meow(3);
            Console.WriteLine("Введите имя пумы: ");
            Puma pum = new(Console.ReadLine());
            MeowCount mt = new(pum);

            Console.WriteLine($"Сколько раз должна рычать пума?");
            mt.meow(int.Parse(Console.ReadLine()));

            Console.WriteLine($"Количество: {mt.Count}");




            Fraction fract1 = new(2, 3);
            Fraction fract2 = new(5);
            Fraction fract3 = new(2, 5);
            Fraction fract4 = new(3, 5);

            Console.WriteLine($"{fract1} * {fract2} = {fract1 * fract2}");
            Console.WriteLine($"{fract1} / {fract3} = {fract1 / fract3}");
            Console.WriteLine($"({fract1} + {fract2}) / {fract3} - {3} = {(fract1 + fract2) / fract3 - 3}");
            Console.WriteLine($"{fract1} - {fract3} = {fract1 - fract3}");
            Console.WriteLine($"{fract3} =? {fract1 * fract4}: {fract3 == fract1 * fract4}");
            Console.WriteLine($"{fract1 * fract3} =? {fract3 / fract2}: {fract1 * fract3 == fract3 / fract2}");


            Console.WriteLine("Введите числитель: ");
            int Num = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите знаменатель: ");
            int Denum = int.Parse(Console.ReadLine());

            Fraction fract = new(Num, Denum);
            FractionD fractd = new(fract);

            Console.WriteLine($"Значение дроби: {fractd.Value()}");

        }



    }
}