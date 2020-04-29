#ifndef RANDOMH
#define RANDOMH

#include <cstdlib>

inline float rand01() {
	return float(rand()) / RAND_MAX;
}

#endif