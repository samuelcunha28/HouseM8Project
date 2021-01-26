import 'package:flutter/material.dart';
import 'package:housem8_flutter/pages/chat_list_page.dart';
import 'package:housem8_flutter/pages/employer_profile_page.dart';
import 'package:housem8_flutter/pages/mate_search_page.dart';
import 'package:housem8_flutter/view_models/chat_list_view_model.dart';
import 'package:housem8_flutter/view_models/employer_profile_view_model.dart';
import 'package:housem8_flutter/view_models/mate_list_view_model.dart';
import 'package:housem8_flutter/widgets/employer_app_bar.dart';
import 'package:housem8_flutter/widgets/employer_menu_drawer.dart';
import 'package:provider/provider.dart';

class EmployerHomePage extends StatefulWidget {
  @override
  _EmployerHomePageState createState() => _EmployerHomePageState();
}

class _EmployerHomePageState extends State<EmployerHomePage> {
  int _currentIndex = 1;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        drawer: NavDrawer(),
        appBar: EmployerAppBar("HouseM8", true),
        body: SafeArea(
          top: false,
          child: IndexedStack(index: _currentIndex, children: <Widget>[
            ChangeNotifierProvider(
              create: (context) => EmployerProfileViewModel(),
              child: EmployerProfileScreen(),
            ),
            ChangeNotifierProvider(
              create: (context) => MateListViewModel(),
              child: MateSearchPage(),
            ),
            ChangeNotifierProvider(
              create: (context) => ChatListViewModel(),
              child: ChatListPage(),
            ),
          ]),
        ),
        bottomNavigationBar: Container(
          decoration: BoxDecoration(
            boxShadow: <BoxShadow>[
              BoxShadow(
                  color: Colors.black54,
                  blurRadius: 3.0,
                  offset: Offset(0.0, 0.75))
            ],
          ),
          child: BottomNavigationBar(
            onTap: (int index) {
              setState(() {
                _currentIndex = index;
              });
            },
            items: const <BottomNavigationBarItem>[
              BottomNavigationBarItem(
                  icon: Icon(Icons.person), label: 'Perfil'),
              BottomNavigationBarItem(
                  icon: Icon(Icons.search), label: 'Procura'),
              BottomNavigationBarItem(
                  icon: Icon(Icons.chat_bubble), label: 'Chat'),
            ],

            currentIndex: _currentIndex,
            selectedItemColor: Color(0xFF93C901),
            //onTap: _onItemTapped,
          ),
        ));
  }
}
