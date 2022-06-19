using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestDeneme.App;
using Xunit;

namespace XUnitTest.Test
{
    public class CalculatorTest
    {
        public Calculator calculator { get; set; }

        public Mock<ICalculatorService> mymock { get; set; }

        public CalculatorTest()
        {
            mymock = new Mock<ICalculatorService>(); //taklit edilecek servis.Artık Calculatorservis çalışmayacak takit edilecek
            this.calculator = new Calculator(mymock.Object);//Calculator taklit edilen mock nesnesiyle beraber ayağa kalktı

            //this.calculator = new Calculator(new CalculatorService()); //mock kullanmazsak bu şekilde servise erişilir.
        }

        [Fact] //Test metodu olduğunu belirtmek için parametre almıyorsa kullanılır. Parametre alıyorsa [Theory]
        public void AddTest()
        {
            ////Arrange : Değişkenler initialize edilir,testi yapılacak clasın nesne örneği oluşturulur.Hazırlık evresi
            //int a = 5;
            //int b = 20;

           // var calculator = new Calculator(); 

            ////Act : İnitialize edilen classlara parametreler verip test edilecek metotların çalıştırılacağı yerdir.

            //var total = calculator.Add(a, b);

            ////Assert : Act ten çıkan sonuç beklenilen sonuç mu? bu test edilir,doğrulanır.
           /* Assert.Equal<int>(25, total);*/ //(beklenen değer,alınan değer).Equal geneic bir metottur.Generic olarak kullanırsak daha efektif olur.

            /*************************************************/

            //dizi içinde fatih varsa true dönecek
            //var names = new List<string>() { "Fatih", "Emre", "Hasan" };
            //Assert.Contains(names, x => x == ("Fatih"));

            // Ali ismi Fatih Çakıroğlunda var mı?
            // Assert.DoesNotContain("Ali", "Fatih Çakıroğlu"); 


            /***********************************/

            //Assert.False(5 < 2);

            //Assert.True("".GetType() == typeof(string));

            /***********************************/

            /* var regex = "^dog";*///dog ile mi başlıyor onu kontrol eder.
                                    //Assert.Matches(regex, "dog fail");

            //Assert.DoesNotMatch(regex, "fail dog");

            /***********************************/

            //Assert.StartsWith("Fatma", "Fatma Ünlü");
            //Assert.EndsWith("Fatma", "Fatma Ünlü");

            /***********************************/

            //verilen dizi boş ise true döner
            //Assert.Empty(new List<string>());
            //Assert.NotEmpty(new List<string>() {"Fatma"});

            /***********************************/

            /* Assert.InRange(10, 2, 20);*/ //değer 2 ile 20 arasında mı
                                            //Assert.NotInRange(10, 2, 20);

            /***********************************/

            //Assert.Single(new List<string>() { "Fatma" });
            //Assert.Single(new List<string>() { "Fatma", "Harun" });
            //Assert.Single<int>(new List<int>() { 1, 2, 3 });

            /***********************************/

            /*Assert.IsType<string>("Fatma");*/ //girilen ifade string tipideyse true değilse false
                                                //Assert.IsNotType<string>(1);

            //Assert.IsNotType<int>("Fatma");

            /***********************************/

            /* Assert.IsAssignableFrom<IEnumerable<string>>(new List<string>());*/// List sınıfı IEnumarableimplement ettiği için referans verbiliyor,yani true döner.

            /*Assert.IsAssignableFrom<object>("Fatih");*/ //girilen değer objeden miras almış olmalı.

            /***********************************/

            //string deger = null;
            //Assert.Null(deger);
            //Assert.NotNull(deger);          

        }    

        [Theory]
        [InlineData(1,6,7)]
        [InlineData(11, 9, 20)]

        public void add_simpleValue_ReturnTotalValue(int a, int b, int expectedTotal)
        {           
            mymock.Setup(x => x.add(a, b)).Returns(expectedTotal); //ICalculator service içerisindeki add metodu çağrılırsa expectedTotal sonucunu ver. Sanallaştırma yapılmış oldu.
            var actualTotal = calculator.add(a, b);

            Assert.Equal(expectedTotal, actualTotal);

            mymock.Verify(x=>x.add(a, b), Times.Once); //bir kere çalışırsa test başarılı olacak. Verify kaç kere çalıştığını ya da hiç çalışmadığını test etmek için kullanılır

           /* mymock.Verify(x => x.add(a, b), Times.AtLeast(2));*/ //en az iki kez çalışmasını kontrol eder.
        }



        [Theory]
        [InlineData(0, 6, 0)]
        [InlineData(11, 0, 0)]

        public void add_zeroValuesValue_ReturnTotalValue(int a, int b, int expectedTotal)
        {
            mymock.Setup(x => x.add(a, b)).Returns(expectedTotal);
            var actualTotal = calculator.add(a, b);
            Assert.Equal(expectedTotal, actualTotal);
        }

        [Theory]
        [InlineData(1,3,3)]
        public void multip_simpleValue_ReturnTotalValue(int a, int b, int expectedValue)
        {
            int actualMultip = 0;
           // mymock.Setup(x => x.multip(a, b)).Returns(expectedValue);
           //kontrolün sadece yukarıda verilen 3,5 değerlerine bağlı kalmaması için bu şekilde yapılır setup.
            mymock.Setup(x=>x.multip(It.IsAny<int>(),It.IsAny<int>())).Callback<int, int>((x,y)=> actualMultip = x*y);

            calculator.multip(a, b); 
            Assert.Equal(expectedValue, actualMultip);


            calculator.multip(2, 5);
            Assert.Equal(10, actualMultip);
        }

        //sıfır değeri aldığında multip fonksiyonunda verilmesi gereken hata mesajı veriliyor mu
        [Theory]
        [InlineData(0, 3)]
        public void multip_zeroValue_ReturnTotalValue(int a, int b)
        {
            mymock.Setup(x => x.multip(a, b)).Throws(new Exception("a=0 olamaz")); //mock üzrinden multip fonk. ndaki hata mesajı taklit edilir.                        
            Exception exception = Assert.Throws<Exception>(()=>calculator.multip(a, b));

            Assert.Equal("a=0 olamaz", exception.Message);
        }



    }
}
