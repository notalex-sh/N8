# N8
> deton8ion

**In progress**

this will be a tool for malware analysis, by essentially logging all actions taken on the file system and over the network

intention is for analysing installer files, but could apply to any executables, will just log a lot of activity

this will also alert on certain actions that can be defined in a policy (maybe YARA rules or similar, atm policy .json shows some examples of activity in common sus dirs)

will use ETW kernel log and also WMI

currently: spawns executable with user priv and gets PID

will do:

 spawn process
 monitor all activity and processes spawned by children/grandchildren
 following execution:
    parse logs and alert on anything violating policy
    potential api integrations with OSINT dbs (abuseipd, virustotal etc)

essentially this will be an open source sandboxing tool alternative

more coming soon