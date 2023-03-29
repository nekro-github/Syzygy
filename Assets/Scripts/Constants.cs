using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants {
    public static readonly ScientificNumber G = new ScientificNumber(6.6743f,-11);
    public static readonly ScientificNumber earthMass = new ScientificNumber(5.9722f, 24);
    public static readonly ScientificNumber earthRadius = new ScientificNumber(6.371f, 6);
    public static readonly ScientificNumber moonMass = new ScientificNumber(7.348f, 22);
    public static readonly ScientificNumber moonRadius = new ScientificNumber(1.7374f, 6);
}

[System.Serializable]
public class ScientificNumber {
    public float num;
    public float exponent;
    public float asFloat { get{return num * Mathf.Pow(10,exponent);}}
    [HideInInspector] public double asDouble {get{ return (double)num * Mathf.Pow(10,exponent); }}
    [HideInInspector] public ScientificNumber inverse {get{ return new ScientificNumber(1/num,-exponent).Validate(); }}
    public ScientificNumber(float num, float exponent) {
        this.num = num; this.exponent = exponent;
        Validate();
    }
    public ScientificNumber(double num, float exponent) {
        this.num = (float)num; this.exponent = exponent;
        Validate();
    }
    public ScientificNumber(int num, float exponent) {
        this.num = (float)num; this.exponent = exponent;
        Validate();
    }
    public ScientificNumber Validate() {
        if (num > 10.0f) {
            num /= 10.0f;
            exponent += 1;
        } else if (num < 1.0f) {
            if (num == 0.0f) return this;
            num *= 10.0f;
            exponent -= 1;
        } else {
            return this;
        }
        return Validate();
    }
    public override string ToString() { return num+"e"+exponent; }
    public ScientificNumber Copy() { return new ScientificNumber(num, exponent); }
    public ScientificNumber Pow(int power) { return new ScientificNumber(Mathf.Pow(num,power),exponent*power); }
    public ScientificNumber Pow(float power) { return new ScientificNumber(Mathf.Pow(num,power),exponent*power); }
    public ScientificNumber Pow(double power) { return new ScientificNumber(Mathf.Pow(num,(float)power),exponent*(float)power); }

    public static ScientificNumber operator +(ScientificNumber a, ScientificNumber b) { return new ScientificNumber(a.asDouble+b.asDouble,1); }
    public static ScientificNumber operator -(ScientificNumber a, ScientificNumber b) { return new ScientificNumber(a.asDouble-b.asDouble,1); }
    public static ScientificNumber operator *(ScientificNumber a, ScientificNumber b) { return new ScientificNumber(a.num*b.num,a.exponent+b.exponent); }
    public static ScientificNumber operator /(ScientificNumber a, ScientificNumber b) { return new ScientificNumber(a.num/b.num,a.exponent-b.exponent); }



    public static ScientificNumber operator *(ScientificNumber a, int b) { return new ScientificNumber(a.num*(float)b,a.exponent); }// int
    public static ScientificNumber operator /(ScientificNumber a, int b) { return new ScientificNumber(a.num/(float)b,a.exponent); }
    public static ScientificNumber operator *(int a, ScientificNumber b) { return new ScientificNumber(((float)a)*b.num, b.exponent); }// int other direction
    public static ScientificNumber operator /(int a, ScientificNumber b) { return new ScientificNumber(((float)a)/b.num, -b.exponent); }

    public static ScientificNumber operator *(ScientificNumber a, float b) { return new ScientificNumber(a.num*b,a.exponent); }// float
    public static ScientificNumber operator /(ScientificNumber a, float b) { return new ScientificNumber(a.num/b,a.exponent); }
    public static ScientificNumber operator *(float a, ScientificNumber b) { return new ScientificNumber(a*b.num, b.exponent); }// float other direction
    public static ScientificNumber operator /(float a, ScientificNumber b) { return new ScientificNumber(a/b.num, -b.exponent); }

    public static ScientificNumber operator *(ScientificNumber a, double b) { return new ScientificNumber(a.num*(float)b,a.exponent); }// double
    public static ScientificNumber operator /(ScientificNumber a, double b) { return new ScientificNumber(a.num/(float)b,a.exponent); }
    public static ScientificNumber operator *(double a, ScientificNumber b) { return new ScientificNumber(((float)a)*b.num, b.exponent); }// double other direction
    public static ScientificNumber operator /(double a, ScientificNumber b) { return new ScientificNumber(((float)a)/b.num, -b.exponent); }
}