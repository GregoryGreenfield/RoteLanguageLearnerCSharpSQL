# RoteLanguageLearner
Written by Gregory Greenfield. 18/11/22

This program is a rote learning tool. It tests the user with their knowledge of German multiple times. It connects to a SQL database of German words, hosted on Docker. It was developed using VSCode on a Mac and Written in C# and SQL.

The program revolves around the RoteNumber: 

RoteNumber = 0 for newly added words
    
RoteNumber++ if the user is correct

RoteNumber-- if the user is not correct
    
if 5<RoteNumber<16 then RoteNumber++
    
if RoteNumber==16 then the user is tested again. If they are correct then RoteNumber = 17 and the word is considered memorised. If they are incorrect, then RoteNumber = 0, as though it were a new word.

RoteNumber can not go below 0.
