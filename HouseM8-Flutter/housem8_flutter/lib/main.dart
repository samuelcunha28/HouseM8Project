import 'dart:io';

import 'package:connectivity/connectivity.dart';
import 'package:data_connection_checker/data_connection_checker.dart';
import 'package:enum_to_string/enum_to_string.dart';
import 'package:flutter/material.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:housem8_flutter/helpers/connection_helper.dart';
import 'package:housem8_flutter/helpers/storage_helper.dart';
import 'package:housem8_flutter/pages/employer_home_page.dart';
import 'package:housem8_flutter/pages/login_page.dart';
import 'package:housem8_flutter/pages/mate_home_page.dart';

import 'enums/roles.dart';

final storage = FlutterSecureStorage();

Future main() async {
  await loadSettings();
  runApp(HouseM8App());
}

Future loadSettings() async {
  await DotEnv().load("env/dev.env");
  HttpOverrides.global = new MyHttpOverrides();
}

class MyHttpOverrides extends HttpOverrides {
  @override
  HttpClient createHttpClient(SecurityContext context) {
    return super.createHttpClient(context)
      ..badCertificateCallback =
          (X509Certificate cert, String host, int port) => true;
  }
}

class HouseM8App extends StatefulWidget {
  @override
  State<StatefulWidget> createState() {
    return _HouseM8AppState();
  }
}

class _HouseM8AppState extends State<HouseM8App> {
  bool isDeviceConnected = false;
  String role;
  int statusCode;
  var internetConnection;

  @override
  void initState() {
    super.initState();

    //Ativar listener para caso a conectividade mude
    internetConnection = Connectivity()
        .onConnectivityChanged
        .listen((ConnectivityResult result) async {
      if (result != ConnectivityResult.none) {
        isDeviceConnected = await DataConnectionChecker().hasConnection;
        if (isDeviceConnected) {
          role = await StorageHelper.readTokenRole();
          statusCode = await ConnectionHelper.checkTokenValidaty();
          setState(() {});
        }
      } else {
        isDeviceConnected = false;
        setState(() {});
      }
    });
  }

  @override
  Widget build(BuildContext context) {
    if (isDeviceConnected) {
      return buildOnlineApp(context);
    } else {
      return buildOfflineApp(context);
    }
  }

  Widget buildOfflineApp(BuildContext context) {
    return MaterialApp(
        debugShowCheckedModeBanner: false,
        title: "HouseM8",
        home: Center(
            child: Scaffold(
                body: Center(
                    child:
                        Stack(alignment: Alignment.center, children: <Widget>[
          Container(
            height: double.infinity,
            width: double.infinity,
            decoration: BoxDecoration(
              color: Color(0xFFE0E0E0),
            ),
          ),
          Container(
            height: double.infinity,
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: <Widget>[
                Container(
                  child: new Image.asset(
                    'assets/images/logo_housem8.png',
                    height: 200.0,
                  ),
                ),
                Container(
                    child: FutureBuilder(
                        future: ConnectionHelper.checkConnection(),
                        builder: (context, snapshot) {
                          if (snapshot.data == true) {
                            return Column(
                              children: <Widget>[
                                Text(
                                  "Iniciando....",
                                  style: TextStyle(
                                      fontSize: 20, color: Color(0xFF5B82AA)),
                                ),
                              ],
                            );
                          } else {
                            return Column(
                              children: <Widget>[
                                Text(
                                  "Dispositivo Offline!",
                                  style: TextStyle(
                                      fontSize: 20, color: Color(0xFF5B82AA)),
                                ),
                              ],
                              // ignore: missing_return
                            );
                          }
                        })),
              ],
            ),
          ),
        ])))));
  }

  Widget buildOnlineApp(BuildContext context) {
    internetConnection.cancel();
    if (statusCode == 200) {
      if (EnumToString.fromString(Roles.values, role) == Roles.M8) {
        return MaterialApp(
            localizationsDelegates: [GlobalMaterialLocalizations.delegate],
            supportedLocales: [const Locale('pt')],
            debugShowCheckedModeBanner: false,
            title: "HouseM8",
            home: MateHomePage());
      } else {
        return MaterialApp(
            localizationsDelegates: [GlobalMaterialLocalizations.delegate],
            supportedLocales: [const Locale('pt')],
            debugShowCheckedModeBanner: false,
            title: "HouseM8",
            home: EmployerHomePage());
      }
    } else {
      return MaterialApp(
          localizationsDelegates: [GlobalMaterialLocalizations.delegate],
          supportedLocales: [const Locale('pt')],
          debugShowCheckedModeBanner: false,
          title: "HouseM8",
          home: LoginScreen());
    }
  }

  @override
  void dispose() {
    if (internetConnection != null) {
      internetConnection.cancel();
    }
    super.dispose();
  }
}
