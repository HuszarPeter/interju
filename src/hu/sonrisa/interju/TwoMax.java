package hu.sonrisa.interju;

public class TwoMax {
    
    // Írasd ki a paraméterben megkapott integereket tartalmazó tömbből a két legnagyobb számot.
    public void PrintTwoLargestNumber(int[] numbers) {
        if (numbers == null)
            return;
        if (numbers.length < 2) 
            return;
        int first = numbers[0];
        int second = numbers[1];
        for (int i = 1 ; i < numbers.length; i++) {
            int value = numbers[i];
            if(value > first) {
                second = first;
                first = value;  
            } else if (value > second) {
                second = value;
            }
        }
        System.out.println("1. " + first + " 2. " + second);
    }

    public static void main(String[] args) {
        TwoMax twomax = new TwoMax();
        
        twomax.PrintTwoLargestNumber(new int[]{1,4,8,9});
        twomax.PrintTwoLargestNumber(new int[]{-5,30,1,2,3,15,10,20});
        twomax.PrintTwoLargestNumber(new int[]{5,-30,-1,-2,-3,-15,-10,-20});
    }
}
