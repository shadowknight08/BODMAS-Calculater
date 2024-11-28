using System;
using System.Data;
using TMPro;
using UnityEngine;


public class Calculator : MonoBehaviour
{
    [SerializeField]
    TMP_InputField expressionField;
    [SerializeField]
    TextMeshProUGUI resultText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearExpressionAndResult()
    {
        expressionField.text = string.Empty;
        int result = 0;
        resultText.text = result.ToString();
    }
    public void CheckResult()
    {
        // Check Though Data Table 
       // resultText.text =  EvaluateByDataTable(expressionField.text).ToString();

        resultText.text = Evaluate(expressionField.text).ToString();

    }

    public Double EvaluateByDataTable(string expression)
    {
        DataTable table = new DataTable();
        object result = table.Compute(expression, "");

        return Convert.ToDouble(result);
    }

   
    public double Evaluate(string expression)
    {
        if (string.IsNullOrEmpty(expression))
        {
            return 0;
        }
        try
        {
            // Handle parentheses first
            while (expression.Contains("("))
            {
                int open = expression.LastIndexOf('(');
                int close = expression.IndexOf(')', open);
                if (close == -1) throw new Exception("Mismatched parentheses!");

                string innerExpression = expression.Substring(open + 1, close - open - 1);
                double innerResult = Evaluate(innerExpression);

                expression = expression.Substring(0, open) + innerResult + expression.Substring(close + 1);
            }

            // Evaluate the expression without parentheses
            return EvaluateSimple(expression);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error: {ex.Message}");
            throw; 
        }
    }

    private double EvaluateSimple(string expression)
    {
        // Split expression into terms for '+' and '-'
        char[] operators = { '+', '-' };
        string[] terms = expression.Split(operators, StringSplitOptions.RemoveEmptyEntries);
        double result = ParseTerm(terms[0]);

        int index = terms[0].Length; // Track operator positions
        for (int i = 1; i < terms.Length; i++)
        {
            char operatorChar = expression[index];
            double nextValue = ParseTerm(terms[i]);

            if (operatorChar == '+') result += nextValue;
            else if (operatorChar == '-') result -= nextValue;

            index += terms[i].Length + 1;
        }

        return result;
    }

    private double ParseTerm(string term)
    {
        // Split term into factors for '*' and '/'
        char[] operators = { '*', '/' };
        string[] factors = term.Split(operators, StringSplitOptions.RemoveEmptyEntries);
        double result = double.Parse(factors[0]);

        int index = factors[0].Length; // Track operator positions
        for (int i = 1; i < factors.Length; i++)
        {
            char operatorChar = term[index];
            double nextValue = double.Parse(factors[i]);

            if (operatorChar == '*') result *= nextValue;
            else if (operatorChar == '/')
            {
                if (nextValue == 0) throw new DivideByZeroException("Cannot divide by zero!");
                result /= nextValue;
            }

            index += factors[i].Length + 1;
        }

        return result;
    }
}


