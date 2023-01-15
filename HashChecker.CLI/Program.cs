using System.CommandLine;
using System.Diagnostics;
using System.Reflection;

RootCommand root = new("Hash Checker Tool");

Command sub_hash = new("hash", "Computes hash of specified data");

Option<string> opt_hash_alg = new("--algorithm", "Specified hash algorithm name");
opt_hash_alg.AddAlias("-a");
opt_hash_alg.AddValidator(result =>
{
    string[] algorithmNames = { "MD5", "SHA1", "SHA256", "SHA384", "SHA512" };
    if(!algorithmNames.Contains(
        result.GetValueForOption(opt_hash_alg)?
        .ToUpperInvariant().Replace("-", "")
        ))
    {
        result.ErrorMessage = "'algorithm' name must be one of 'MD5', 'SHA1', 'SHA256', 'SHA384', or 'SHA512'.";
    }
});

Option<bool> opt_hash_dt = new("--detail", () => true, "Print the result verbosly");
opt_hash_dt.AddAlias("-d");

Option<bool> opt_hash_sm = new("--simple", () => false, "Print hash value only");
opt_hash_sm.AddAlias("-s");

Argument<string> arg_hash_file = new("FILE", "A file for computing a hash value");

sub_hash.AddArgument(arg_hash_file);
sub_hash.AddOption(opt_hash_alg);
sub_hash.AddOption(opt_hash_dt);
sub_hash.AddOption(opt_hash_sm);

sub_hash.SetHandler((file, algorithm, detail, simple) =>
{
    file = Path.GetFullPath(file);
    Console.WriteLine($"  FILE      : {file}");
    Console.WriteLine($"--algorithm : {algorithm}");
    Console.WriteLine($"--detail    : {detail}");
    Console.WriteLine($"--simple    : {simple}");

}, arg_hash_file, opt_hash_alg, opt_hash_dt, opt_hash_sm);

root.AddCommand(sub_hash);

root.Options.Where(opt => opt.Name == "--version").FirstOrDefault()?.AddAlias("-v");

root.Invoke(args);
