using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Cormei.Core.Global
{
    public static class env
    {
        // EL KEY DE PRODUCCIÓN
//         public static readonly string ApkPrivateKeyPem= @"-----BEGIN PRIVATE KEY-----
// MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQDMolyeKys9/jAw
// S7swkP+xepZ7X8Y6unCmE/VA/JQr3AUL9/q3f0yFDvO8ZmmzN6C95yF74pDu5JZn
// iO4N6wlBXDpSJvZhiEhvgivc18sEVknEzvvsLnxw8dwrSTUzNMZlYg/gCtKzi4zo
// FIURdWVlQJl4IF5f5QmIfFhJ9YHCe308FuJFjSgAU0nnTgQjBEYZdkrneKkkA/7z
// k3bQ4TiZqIpaDplxJyGdLIdMW9+MXJsOOkbOKaR8ezkPbPFgJ32B4KDaVg1KKb8M
// 8xAOkA1Sgu3eusalwDR31PXyy4V+61cBFr2Dq4r1WOpzxcqUUekrh0SKIFs2DVzZ
// Adykal2NAgMBAAECggEAL20t1AajRgHkfzz+DFp+uE52E+/1jGTreMJFZCNl3+hl
// TwnRFLgvLuNxP+fodX3eBkVMMF8pQgS+iw5kRduEUJ6kcafQEHi6eQWNTujAh6fL
// /w4NuwZnFvInGe0kpFKF7LdWgJFhlfUw2hTvZkV0E4xuRTYTSVxW4kgXWkTJJx53
// XWiRrBS+FdS63TgsUf+RRyHPh4U96WhB7p8qYWve6baEBiEkmZ1HGTuNP2aPbZiM
// CZGj+6BQftgRNQIdeJeyRR+ajkZZNLvksQ2c/cYphbBOg9YGAHGgMCD0BWoIXU00
// gHtLG7W5KWuvU8jKxu6HCMFhZ/15WoWnPut6Sbz8rQKBgQD2Xi2uHJpqFGlSSQGK
// 8puqH5nF+5qLfG2GLyAwpTRBsc34bkBtcA1v1GWVU5S9BqmRmiXQi7sspfiEfI5K
// pwXM6T53kHTLwY2s1P5ZPsdlv4y51tDpubHzZCAZ+sa4aH8j0+aavpdiruyamZ+c
// 0KO3sXQ9G8B2kEyIuC0LeIOSGwKBgQDUonvaB29TehmV9yMalT/zuX5K4uv/GoxR
// 2lNVzS+p+Qm3Rrbzg0SpMmRfhorq1UCI9EvJCxgFNYrg+8hHC9U1EvbPGm/I458H
// xRJkzxcW6oXQuROuZXan7RL0TNzGoHMlrnbHdAMrnBzqYZ8716XM5wfI0/luGCj9
// /BOle0OJdwKBgAPkCt6kDwA74EnvYznZiQHPy/LnfVJUnbovxldeVIvxDeqtexD/
// YwbwGMaZ8lBl6YmmIJDBlCwpVtpoySp+psXMrTLgMigBn4YqOvYX2rhNpSBONzBp
// wVixrRwb7MV+yPs83nh1KBjj4FK0uGOWm+9LuyU36fs1XjAOkI+M0K8fAoGBAKjT
// 1eh4DMFH6fazQdECfcODHS5SXDSxnIYFBjPS9axMSpWVdP+F00dP9ngOOFBaP6I9
// 7cpTn5/LMliOMSeP0poQU5x+jhEAAa3GoGMgNmIAlXzy5hiLARo11t3YBVdD4Akk
// K7UTfMzsZ90E48w83a0CJqEjBSGP10RKGENEyovjAoGACDb3fwaLmEtu2mJjDonQ
// 4aZSbtg1ObvQ9oXJk0XAASU9fCNaSGy0+Xhir9kr2J2asw014QoTkIeDmTFZDc07
// Y6PDluG+BLTi8lZFoijTMrS/um56/Hz4M3KtYsWropZQLn/RlC91FtlvQ2wKggeG
// U1bHGrwwV6Fa36XPNRf45SM=
// -----END PRIVATE KEY-----";
    public static readonly string ApkPrivateKeyPem= @"-----BEGIN PRIVATE KEY-----
MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQC8e/Op+S5iRmi7
IAloCNyWGElboNb/2u0E+pDwCAvuRKaA6UF1XUiid2q3FNH0rKevLnvBLOdyv7i+
Os3HCA5ipwq4HnREBG+oWIyg3QEu9+40LHgRhURFXjDzMCPJZeDIKyZpaqdblKz7
00XldLMdE1q7e9jFcPk1P3NoPgH1my5bWqm2Q8w4CulhWeizU/AQFiqifbyT+AfT
UirJI1GCRy4lNy40AAXSNELAGC2bJpkHuRoGABTVl/P0ZLS+XTdTWzDQJxMfnwk/
jniGKDTMlpieRwr2TxcUrrc2PKmUp1u2kTJfOr3NYrJ1gr+ENdBAUYrZUpHVggRZ
57aLpLAhAgMBAAECggEANaGve7YDlMMkGdrL0mZzHeJbPU9O3g6VUs1ZYHNpsaqn
FhdJIPQiLth77QxnkY9vR1GatSTp1qTnFHn3A6MvNLMDQdColJIOk4Bew4cfRxYV
GZNtts6M1pAurYQNKslqZIqiq2xForFzZ1R+lwPV03gmYSKkU8oJnzUmyYqbVB8O
Z9FtNPh8t1Eh9EBZ2F03qwxRtc3pXcT5YJsPlBNbjLvdRSLeAd+OOU6YHmuKBJUU
/9aUHkJpT8lSJlI9JlpIvuGTroQziV+UlFYri5AI1UY8Oy9t5NHHQJp5mNQqtqGr
gUoava8SSGOZWR0rO1rEiaoO531x0qTIUvqZ8eB3YQKBgQDuzXqPmYenA4bg8d8N
KGpziIF94UWbAwrws+wRHklLG9fFC3kd5I1D4H5gM5bZAgD1ikiCUhjPZrdvUH1J
SxC/gSDFbBfiE0A+CYf46H0kjczhKYBejc1pxeiHNrw9izfQdAfmSMLD+4lQPtif
kQFczsgiaimDosL1Z4TEj1DMRwKBgQDKDs+rUlxbBYpioxDLK0Sj18602kOsjYtb
y8MCSWW59hfabOCdeqSBr2uf0RhVJUuBmLyleWMooQfO/v9sbkErJeHdt2rd1b6y
xrxebNtvf+0rTRRN5rLuns/uf14bt458LGJ5VpTAD0UY5WpB4ORH5G6aYggXF7+t
NOZWTaycVwKBgCmyIQs/wGrN99gBA3Q1sViY5htHoTKutlNO5xIaGqwNoYAbP7uT
c4L9iLNdbqJXcjltOrnegcx839yEzktk2vjFICa4d/cWa2opmd9BINCoHbTW3T4l
Pk+QqcgqK3YHf+hayiJQZAl6OwCS6Jn75KQCyvkPVk5Qtf/JYmo7p9zzAoGAB3bW
mxiWytIAk7Y8Z5T4myjcvPeXKmesLz7uvEXj2SPLK8l73rVqIFRPoH7D7FlNWcM/
rMk4LYcNV8s5ulK0W5ixqFQfQYSk38vRp5dT3+GU7FWTm3EbSQ9a1Q+ldVapj9K/
7IWTRykJP2syKq7ZIALDRza538iDzy02GRgTeU8CgYEA0fRInLjQ/neNkN0OSubA
ympgGOTxGvEDmkQBwmYyhdgvB1Wkh3XXQkzeaXj1jxMHjNOM7hsxaEk1fzGYuDa9
+uTSc3NCK455hgbKtRxeDDWbQONXABYc5vZHQ1vg9IuTX8Y4pClQB3H47okxd1zh
FUVLm+ydJ2bafk/jA1C8iDo=
-----END PRIVATE KEY-----";

        public static void FirmarPeticion(HttpRequestMessage request)
        {
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            string datosAFirmar = $"CormeiApkSecretData-{timestamp}";

            using var rsa = RSA.Create();
            rsa.ImportFromPem(ApkPrivateKeyPem);

            byte[] dataBytes = Encoding.UTF8.GetBytes(datosAFirmar);
            byte[] signatureBytes = rsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            string selloDigital = Convert.ToBase64String(signatureBytes);

            // Inyectamos las cabeceras directamente a la petición que nos enviaron
            request.Headers.Add("X-Apk-Timestamp", timestamp);
            request.Headers.Add("X-Apk-Signature", selloDigital);
        }
    }
}

