import 'package:flutter/material.dart';
import 'package:housem8_flutter/pages/mate_profile_page.dart';
import 'package:housem8_flutter/view_models/chat_list_view_model.dart';
import 'package:housem8_flutter/view_models/job_post_list_view_model.dart';
import 'package:housem8_flutter/view_models/mate_profile_view_model.dart';
import 'package:housem8_flutter/widgets/mate_app_bar.dart';
import 'package:housem8_flutter/widgets/mate_menu_drawer.dart';
import 'package:provider/provider.dart';

import 'chat_list_page.dart';
import 'job_post_search_page.dart';

class MateHomePage extends StatefulWidget {
  @override
  _MateHomePageState createState() => _MateHomePageState();
}

class _MateHomePageState extends State<MateHomePage> {
  int _currentIndex = 1;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        drawer: NavDrawer(),
        appBar: MateAppBar("HouseM8", true),
        body: SafeArea(
          top: false,
          child: IndexedStack(index: _currentIndex, children: <Widget>[
            ChangeNotifierProvider(
              create: (context) => MateProfileViewModel(),
              child: MateProfileScreen(),
            ),
            ChangeNotifierProvider(
              create: (context) => JobPostListViewModel(),
              child: JobPostSearchPage(),
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
            selectedItemColor: Color(0xFF39A3ED),
          ),
        ));
  }
}
