import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

class MateSearchCard extends StatelessWidget {
  final String url;
  final String userName;
  final int range;

  const MateSearchCard({Key key, this.url, this.userName, this.range})
      : super(key: key);

  @override
  Widget build(BuildContext context) {
    double width = MediaQuery.of(context).size.width;
    double height = MediaQuery.of(context).size.height;
    return Container(
        width: width,
        height: height / 1.7,
        alignment: Alignment.center,
        decoration: BoxDecoration(
            shape: BoxShape.rectangle,
            image: DecorationImage(
                image: NetworkImage(url),
                onError: (errDetails, stack) {},
                fit: BoxFit.cover),
            borderRadius: BorderRadius.circular(15)),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.end,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Container(
                alignment: Alignment.centerLeft,
                width: width,
                height: height / 10,
                decoration: BoxDecoration(
                    boxShadow: <BoxShadow>[
                      BoxShadow(
                          color: Colors.black54,
                          blurRadius: 2.0,
                          offset: Offset(0.0, 0.75))
                    ],
                    shape: BoxShape.rectangle,
                    color: Colors.white,
                    borderRadius: BorderRadius.circular(15)),
                child: ButtonTheme(
                  height: height / 10,
                  minWidth: width,
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(15),
                  ),
                  child: RaisedButton(
                    padding: EdgeInsets.only(top: 12.0, left: 20.0),
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.start,
                      crossAxisAlignment: CrossAxisAlignment.stretch,
                      children: [
                        Text(
                          '' + userName,
                          style: TextStyle(
                              color: Colors.black,
                              fontWeight: FontWeight.bold,
                              fontSize: 17),
                        ),
                        SizedBox(
                          height: 5,
                        ),
                        Text(
                          "Distância : " + range.toString() + " KM",
                          style: TextStyle(color: Colors.black, fontSize: 12),
                        ),
                      ],
                    ),
                    color: Colors.white,
                    onPressed: () {
                      print("Ver Mate!");
                    },
                  ),
                )),
          ],
        ));
  }
}
