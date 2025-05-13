# N8
```
                                            c=========e
                                                 H
   ____________                            _,,___H____
  (__((__((___()                          //|         |
 (__((__((___()()________________________// | detoN8  |
(__((__((___()()()-----------------------'  |_________|
```
> deton8ion 🧨

**In progress, I know my C# is not good**

A lightweight, single-binary tool that spins up a **kernel + user-mode ETW session**, launches a target executable, and records the target’s behaviour for post-run triage.

## Usage

**Windows only *obviously*, `net9.0`**

### Build

```bash
# 1) install the .NET 9 SDK (Preview)  
#    https://dotnet.microsoft.com/download/dotnet/9.0

# 2) clone/download the repo and go inside it the repo
cd n8/src

# 3) restore packages and build a self-contained release binary
dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true

# 3.5) OR just run it with dotnet run
```

The executable will be written to:

```
bin\Release\net9.0\win-x64\publish\n8.exe
```


---

### Run

> **Administrator privileges required** (ETW kernel providers).

```powershell
# minimal run
.\n8.exe --target "C:\Samples\malware.exe"

# verbose run (prints every captured artefact)
.\n8.exe --target "C:\Samples\malware.exe" --verbose
```

| Switch      | Description                                             | Required |
|-------------|---------------------------------------------------------|:--------:|
| `--target`  | Full path to the executable you want to observe         | ✓ |
| `--verbose` | Print the full event breakdown (reads, deletes, etc.)   |  |

The program starts the ETW session, launches your target, waits for it (and any children) to exit, then prints an **Execution Summary**.

## Captures

| Category / Artefact                         | Normal Mode | Verbose Mode |
|---------------------------------------------|:-----------:|:------------:|
| **Processes started / stopped**             | ✓ | ✓ |
| **Images loaded** (`.dll`, etc.)            | ✓ | ✓ |
| **Drivers loaded** (`.sys`)                 | ✓ | ✓ |
| **Services installed** (event 7045)         | ✓ | ✓ |
| **Scheduled tasks registered** (event 106)  | ✓ | ✓ |
| **Named pipes created** (`\\pipe\\…`)       | ✓ | ✓ |
| **IPs inbound** (TCP recv / UDP recv)       | ✓ | ✓ |
| **IPs outbound** (TCP connect/send, UDP send) | ✓ | ✓ |
| **Domains queried** (DNS event 3006)        | ✓ | ✓ |
| **File writes**                             | ✓ | ✓ |
| **File reads**                              | ✗ | ✓ |
| **File deletes**                            | ✗ | ✓ |
| **Registry keys created**                   | ✓ | ✓ |
| **Registry keys opened**                    | ✗ | ✓ |
| **Registry keys deleted**                   | ✗ | ✓ |
| **Registry values set**                     | ✓ | ✓ |
| **Registry values deleted**                 | ✗ | ✓ |
| **WMI queries** (event 5858)                | ✗ | ✓ |

## TODO
- [ ] Output file
- [ ] Spawn process with command line type args to allow scripts etc to be sandboxed
- [ ] Capture command line args
- [ ] Capture process + file integrity
- [ ] API integration in analysis
- [ ] Policy file for alerts on sensitive activity
- [ ] Make my code gooder
- [ ] other stuff i forgot about
- [ ] maybe make some sort of agent/listener setup
- [ ] behavioural analysis?
