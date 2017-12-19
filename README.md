# SubProject_2

For some reason the Webservice Layer project file named WebService in the solution is wrongly named Subproject_2 in this github repository. 

Note til aflevering:
Når databasen ligger på wt-220.ruc.dk kan SOVA ikke tilgå dataen. Under Henriks vejledning har vi i stedet for yderligere troubleshooting lagt et database-dump på google drev, som kan hentes og importeres i egen localhost. Denne fil kan findes med linket:
https://drive.google.com/open?id=1D0YC3iwIe-Ex_GBiw8gfDUqeJtzuDuvd
SOVAContext.cs skal ændres i metode-override OnConfiguring, for at de rigtige login oplysninger og selvvalgte databasenavn tages i brug. Databasen fungerer dog for sig selv og kan tilgås manuelt på wt-220.ruc.dk serveren under raw9.
