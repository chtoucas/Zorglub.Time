Les échelles de temps
==============================

Les échelles de temps modernes
------------------------------

D'après [François Vermotte](http://perso.utinam.cnrs.fr/~vernotte/echelles_de_temps.pdf),
augmenté de commentaires personnels.

### Temps Universel (UT)

La seconde est la 1/86400-ème partie du jour solaire moyen.
Le temps Universel UT est le temps solaire moyen pour le méridien origine
augmenté de 12 heures.

- UT0 : temps universel brut, exactitude de l'ordre de 0,1 seconde.
- UT1 : temps universel brut corrigé (2 mois plus tard), avec une
   incertitude de l'ordre de 0,1 ms.
- UT1R : temps universel régularisé.
- UT2 : temps universel régularisé.

### Temps des Éphémérides (TE)

La seconde est la fraction 1/31.556.925.9747 de l'année tropique pour
1900 janvier 0 à 12 heures de temps des éphémérides.

### Temps Atomique International (TAI)

Il s'agit de l'échelle de temps à l'heure actuelle la plus précise.
Sa mesure implique la coordination entre plusieurs centaines d'horloges
atomiques réparties dans différents endroits du globe.

### Temps Terrestre (TT)

Le Temps Terrestre est une échelle abstraite dont une réalisation
concrète est donnée par la formule TT = TAI + 32.184 secondes.

### Temps Universel Coordonné (UTC) à la base de notre temps civil

Compromis entre UT1 et TAI. On suit le TAI mais pas tout à fait sinon
on s'éloignerait progressivement de l'échelle de temps qui rythme
naturellement notre vie, c'est-à-dire le Temps Universel qui par
définition s'accorde avec le jour solaire moyen.
UTC est telle que
* |UTC - UT1| < 0,9 seconde.
* |UTC - TAI| = nombre entier de secondes SI.
Le temps local (ou temps civil) est l'UTC dans une zone géographique
donnée(fuseau horaire) éventuellement corrigé en fonction de la saison
(heure d'hiver, heure d'été).


Prog
----

https://docs.microsoft.com/en-us/dotnet/api/system.buffers.binary.binaryprimitives
https://docs.microsoft.com/en-us/dotnet/api/system.net.ipaddress.hosttonetworkorder
https://docs.microsoft.com/en-us/dotnet/api/system.bitconverter
https://docs.microsoft.com/en-us/dotnet/api/system.numerics.bitoperations


Références
----------

Liste très complète d'échelles de temps.
- https://www.ucolick.org/~sla/leapsecs/timescales.html
- https://www.ucolick.org/~sla/leapsecs/

- http://leapsecond.com/
- http://leapsecond.com/ntp/
- https://github.com/google/unsmear
- https://developers.google.com/time/
- [Official U.S. Time](https://time.gov/)
- [NTP.Org](https://www.ntp.org/)
- [NTP Pool Project](https://www.ntppool.org/en/)

https://kb.meinbergglobal.com/kb/time_sync/ntp/leap_second_smearing/ntp_leap_smearing_test_results
https://www.nwtime.org/ntps-refid/

## Implémentations

- [Implémentation de référence](https://github.com/ntp-project/ntp)
- [ntpdig](https://docs.ntpsec.org/latest/ntpdig.html)
- [Ntimed](https://github.com/bsdphk/Ntimed)

### SNTP clients
- [Java Android](https://android.googlesource.com/platform/frameworks/base/+/master/core/java/android/net/SntpClient.java)
- [Go](https://github.com/beevik/ntp/)

Others:
- [How to Query an NTP Server using C\#?](https://stackoverflow.com/questions/1193955/how-to-query-an-ntp-server-using-c)
- [Haskell](https://hackage.haskell.org/package/hsntp)
- [Perl](https://metacpan.org/pod/Net::SNTP::Client)
- [Python](https://github.com/cf-natali/ntplib)
- [GuerrillaNtp](https://github.com/robertvazan/guerrillantp)
- [ios-ntp](https://github.com/jbenet/ios-ntp/)
- [C\# SNTP client in .NET Micro Framework](https://github.com/vbocan/sntp-client)

- [Facebook time](https://github.com/facebook/time) and
  [explanations](https://engineering.fb.com/2020/03/18/production-engineering/ntp-service/)
