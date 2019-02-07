In the "LogMessage" method the "message" parameter (booleaning) is repeated, the name must be unique.
The variable "_initialized" is not used.
The variable "LogToDatabase" does not share the good practices of the names used.
The date log code is tripled.
It is an error that you always write in all the destinations, you must respect the configured.
Objects are not downloaded and connections are not closed. 
It is missing to contemplate in the file that records in disk the level of logueo, it is not known if it is warning, error or text,
Besides that everything is recorded together, there is no difference where the logueo begins or ends.
The access to the configuration is repeated three times, it is better to store it in a variable.
The method had too many lines, it is necessary to divide it in smaller methods.
A code without comments or documentation is not a good practice.
You have to use "using" well.