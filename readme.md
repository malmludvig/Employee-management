Tjena Tim!

Här är min inlämningsuppgift.
Jag har inkluderat min employees-csv fil på gitHub. 

Min extra funktionalitet som jag har lagt till i uppgiften för VG-kravet är framförallt:

-Alla id:n slumpas från Guid, och tilldelas till Employee.Id som en sträng
-Alla lösenord använder MD5 när de sparas. Lösenorden som väljs sparas med andra ord aldrig i klartext.

Kända buggar:

Om man loggar in på UserUi som user och sedan byter namn på sig själv så kommer inte funktionen "" att fungera längre. Jag vet hur man ska lösa det men jag hann inte.

