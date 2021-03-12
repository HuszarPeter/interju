package hu.sonrisa.interju;

public class FizzBuzz {
    
    // Írasd ki az első 100 pozitív egész számot (1..<=100) a következő szabályok mentén:
    // 1. Ha az adott szám 3-al osztható maradék nélkül akkor a szám helyett azt írd ki, hogy 'Fizz'
    // 2. Ha az adott szám 5-el osztható maradék nélkül akkor a szám helyett azt írd ki, hogy 'Buzz'
    // 3. Ha az adott szám 3-al és 5-el is osztható maradék nélkül, akkor 'FizzBuzz' legyen kiírva a szám helyett
    // 4. Minden más esetben magát a számot kell kiírnod
    public void PrintFizzBuzz() {
        for(int i = 1; i <= 100; i++) {
            String text = "";
            if (i % 3 == 0) {
                text = "Fizz";
            }
            if (i % 5 == 0) {
                text += "Buzz";
            }
            if(text.length() == 0) {
                text = Integer.toString(i);
            }

            System.out.println(text);
        }
    }

    public static void main(String[] args) {
        FizzBuzz fizzBuzz = new FizzBuzz();
        fizzBuzz.PrintFizzBuzz();
    }
}
