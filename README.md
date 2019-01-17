# WhoIsCrawler

A .NET core application for getting WhoIs data. Saves found data as a json.
### Usage:
* If you want to run the code from Visual Studio, or something similiar, just change the configuration in the Program.cs file. in the `InitConfig` method.
* If you want to run from command line *as is*: 
1. Compile into Release Build on Visual Studio, or something similiar.
2. Call the built application like this:
 
 if compiled from the `master` branch:
     
     `dotnet [path_to_dll] [domains_input_file_name] [domains_output_fn] [registrars_output_fn] [raw_output_fn] [failed_log_fn] [(Optional)proxy_address] [(Optional)proxy_username] [(Optional)proxy_password]`

 if compiled from the `get-all-info-as-json` branch:
     
     `dotnet [path_to_dll] [domains_input_file_name] [domains_output_fn] [failed_log_fn] [(Optional)proxy_address] [(Optional)proxy_username] [(Optional)proxy_password]`
