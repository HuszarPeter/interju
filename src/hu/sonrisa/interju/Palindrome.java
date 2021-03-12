package hu.sonrisa.interju;

public class Palindrome {
    
    // A metódusod döntse el, hogy a paraméterként megkapott string plaindróma-e vagy sem.
    // A visszatérési érték true legyen ha az str palindróma, false ha nem az.
    public boolean IsPalindrome(String str) {
        return false;
    }

    public static void main(String[] args) {
        Palindrome palindrome = new Palindrome();

        System.out.println(palindrome.IsPalindrome("apa"));
        System.out.println(palindrome.IsPalindrome("indulagorogaludni"));
    }
}
