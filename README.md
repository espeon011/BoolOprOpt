# BoolOprOpt

## 定式化

$a$, $b$ を0-1 決定変数とする. 
このとき以下のような制約を持つ決定変数 $c$ の定め方を説明する. 

- AND: $c = a \land b$
- OR: $c = a \lor b$
- NAND: $c = a \uparrow b$
- NOR: $c = a \downarrow b$
- XOR: $c = a \veebar b$
- XNOR: $c = (a \Leftrightarrow b)$

### AND

$a \land b \Rightarrow c$ と $c \Rightarrow a \land b$ を表現すればよい. 
($x \Rightarrow y$ は $x \leq y$ で書けることに注意)

- $a + b \leq 1 + c$
- $2 c \leq a + b$

### OR

$a \lor b \Rightarrow c$ と $c \Rightarrow a \lor b$ を表現すればよい. 

- $a + b \leq 2 c$
- $c \leq a + b$

### NAND

AND の定式化における $c$ を $1 - c$ に置き換えればよい. 

- $a + b \leq 1 + (1 - c)$
- $2 (1 - c) \leq a + b$

あるいは AND で定式化した $c$ に対して $1 - c$ を用いる. 

### NOR

OR の定式化における $c$ を $1 - c$ に置き換えれば良い. 

- $a + b \leq 2 (1 - c)$
- $1 - c \leq a + b$

あるいは OR で定式化した $c$ に対して $1 - c$ を用いる. 

### XOR

$(a = 0 \land b = 0) \Rightarrow c = 0$, $(a = 1 \land b = 1) \Rightarrow c = 0$, $(a = 1 \land b = 0) \Rightarrow c = 1$, $(a = 0 \land b = 1) \Rightarrow c = 1$ を全て表現する. 

- $(1 - a) + (1 - b) \leq 1 + (1 - c)$
- $a + b \leq 1 + (1 - c)$
- $a + (1 - b) \leq 1 + c$
- $(1 - a) + b \leq 1 + c$

### XNOR

XOR の定式化における $c$ を $1 - c$ に置き換えれば良い. 

- $(1 - a) + (1 - b) \leq 1 + c$
- $a + b \leq 1 + c$
- $a + (1 - b) \leq 1 + (1 - c)$
- $(1 - a) + b \leq 1 + (1 - c)$

あるいは XOR で定式化した $c$ に対して $1 - c$ を用いる. 

## 実行結果

Google OR-Tools の CP-SAT ソルバーで目的関数を設定せずに実行可能解を全て求めるオプションをオンにして実行. 

```shell
$ dotnet run
Solution #1: time = 0.00092 s
  A = 0, B = 0
  (A)AND(B) = 0  (A)OR(B) = 0  (A)NAND(B) = 1  (A)NOR(B) = 1  (A)XOR(B) = 0  (A)XNOR(B) = 1
Solution #2: time = 0.01305 s
  A = 0, B = 1
  (A)AND(B) = 0  (A)OR(B) = 1  (A)NAND(B) = 1  (A)NOR(B) = 0  (A)XOR(B) = 1  (A)XNOR(B) = 0
Solution #3: time = 0.01311 s
  A = 1, B = 1
  (A)AND(B) = 1  (A)OR(B) = 1  (A)NAND(B) = 0  (A)NOR(B) = 0  (A)XOR(B) = 0  (A)XNOR(B) = 1
Solution #4: time = 0.01313 s
  A = 1, B = 0
  (A)AND(B) = 0  (A)OR(B) = 1  (A)NAND(B) = 1  (A)NOR(B) = 0  (A)XOR(B) = 1  (A)XNOR(B) = 0
Number of solutions found: 4
```
