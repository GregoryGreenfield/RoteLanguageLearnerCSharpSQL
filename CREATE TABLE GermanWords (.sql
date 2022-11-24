/* Creates the table German words
CREATE TABLE GermanWords (
WordID int,
EnglishWord varchar(255),
GermanWord varchar(255),
Gender varchar(10),
WordType varchar(255),
NudgingNotes varchar(255),
RoteNumber int,
PRIMARY KEY (WordID)
);*/

/*Determine SQL version*/
SELECT @@VERSION

/*Enter new data into GermanWords*/
INSERT INTO GermanWords (WordID, EnglishWord, GermanWord, Gender, WordType, NudgingNotes, RoteNumber) VALUES (3,'woman','Frau','f','noun','Frau von Welt',7), (4,'money','Geld','n','noun','Geld heiraten',4), (5,'money tree','Geldbaum','m','noun','Ents',2), (6,'tree','Baum','m','noun','Quickbeam',5), (7,'song','Lied','n','noun','Mahler',2), (8,'world','Welt','f','noun','Mother earth',2), (9,'attack (noun)','Angriff','m','noun','Griffon',2), (10,'to attack (verb)','angreifen','-','verb','Verbised Griffon',1);

/* shows entirety of the table */
SELECT * FROM GermanWords;

/*selects a column*/
SELECT RoteNumber FROM GermanWords; 

/*Shows all RoteNumbers between a certain value*/
SELECT * FROM GermanWords WHERE RoteNumber > 6 and RoteNumber<16;

/*Gives RoteNumber value for specific WordID*/
SELECT RoteNumber FROM GermanWords
WHERE WordID = 1;

/* adds 1 to all RoteValues within limits*/
UPDATE GermanWords
SET RoteNumber += 1
WHERE RoteNumber>6 AND RoteNumber<17;

/*Sets RoteNumber to 0*/
UPDATE GermanWords
SET RoteNumber = 1
WHERE WordID = 4;

/*Adds 1 to RoteNumber*/
UPDATE GermanWords
SET RoteNumber += 1
WHERE WordID = 2; /*C# needs to specify the WordID*/ 

/*Subtracts 1 from RoteNumber*/
UPDATE GermanWords
SET RoteNumber -= 1
WHERE WordID = 2; /*C# needs to specify the WordID*/ 

/*Find out number of word entries*/
select max(WordID)
from GermanWords;