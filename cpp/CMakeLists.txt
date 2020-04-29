project(WERT)

set(SRC_FILES
    ray.h
    vec3.h
    stb_image.h
    stb_image_write.h
    hitable.h
    hitable_list.h
    sphere.h
	moving_sphere.h
    camera.h
	material.h
	random.h

    stb.cpp
    main.cpp
)

add_executable(wert ${SRC_FILES})